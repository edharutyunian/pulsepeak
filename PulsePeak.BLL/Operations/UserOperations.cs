using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using PulsePeak.Core.BLLContracts;
using PulsePeak.Core.Entities.Contacts;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Utils.Extensions;
using PulsePeak.Core.Utils.Validators;
using PulsePeak.Core.ViewModels.AuthModels;
using PulsePeak.Core.ViewModels.UserViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;
using PulsePeak.DAL.RepositoryAbstraction;


namespace PulsePeak.BLL.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IRepositoryHandler repositoryHandler;

        private readonly IMapper mapper;
        private string errorMessage = string.Empty;
        private readonly IConfiguration configuration;
        private UserManager<UserBaseEnttity> UserManager { get; set; }
        private ConcurrentDictionary<long, string> refreshTokens = new ConcurrentDictionary<long, string>(); //TODO: Replace with secure storage mechanism (e.g. Database)


        public UserOperations(IServiceProvider serviceProvider, UserManager<UserBaseEnttity> userManager, IConfiguration configuration)
        {
            UserManager = userManager;
            this.repositoryHandler = serviceProvider.GetRequiredService<IRepositoryHandler>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.configuration = configuration;
        }
        public async Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest)
        {
            var response = new AuthResponse();

            var user = await UserManager.FindByNameAsync(authenticationRequest.UserName);

            if (user == null)
            {
                response.Success = false;
                response.Errors.Add("User not found.");
            }
            else if (!await UserManager.CheckPasswordAsync(user, authenticationRequest.Password))
            {
                response.Success = false;
                response.Errors.Add("Wrong Password.");
            }
            else
            {
                // User authenticated, create claims and tokens
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                // Add role claim if user has roles (assuming IdentityRole)
                var roles = await UserManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var accessToken = GenerateToken(claims);
                var refreshToken = GenerateRefreshToken();

                // Store refresh token securely (replace with storage logic)
                StoreRefreshToken(user.Id, refreshToken);

                response.Success = true;
                response.Token = accessToken;
                response.RefreshToken = refreshToken;
            }

            return response;
        }

        public async Task<UserBaseEnttity> CreateUser(UserModel userModel)
        {
            if (!await IsValidUserModel(userModel))
            {
                throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
            }

            try
            {
                var user = this.mapper.Map<UserBaseEnttity>(userModel);
                user.Contacts.Add(new ContactBaseEntity
                {
                    UserId = user.Id,
                    User = user,
                    Active = true,
                    Type = Core.Enums.ContactType.EmailAddress,
                    Value = userModel.EmailAddress
                });
                user.Contacts.Add(new ContactBaseEntity
                {
                    UserId = user.Id,
                    User = user,
                    Active = true,
                    Type = Core.Enums.ContactType.PhoneNumber,
                    Value = userModel.PhoneNumber
                });
                this.repositoryHandler.UserRepository.Add(user);
                return user;
            }
            catch (RegistrationException e)
            {
                throw new RegistrationException(errorMessage, e);
            }
        }

        public async Task<CustomerRegistrationResponse> CustomerRegistration(
            CustomerRegistrationRequest customerRegistrationRequest)
        {
            if (customerRegistrationRequest == null)
            {
                throw new RegistrationException("Bad Request. "); // TODO: change the exception message
            }

            if (!customerRegistrationRequest.Customer.FirstName.IsValidName(out errorMessage) ||
                !customerRegistrationRequest.Customer.LastName.IsValidName(out errorMessage))
            {
                throw new RegistrationException(errorMessage);
            }

            if (!Validator.IsValidBirthDate(customerRegistrationRequest.Customer.BirthDate, out errorMessage))
            {
                throw new RegistrationException(errorMessage);
            }

            await using var transaction = await this.repositoryHandler.CreateTransactionAsync();
            try
            {
                // not sure on this tbh
                var user = await this.CreateUser(customerRegistrationRequest.Customer.User);
                user.Type = UserType.CUSTOMER;
                user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                user.Active = true;

                var customer = new CustomerEntity
                {
                    UsertId = user.Id,
                    User = user,
                    FirstName = customerRegistrationRequest.Customer.FirstName,
                    LastName = customerRegistrationRequest.Customer.LastName,
                    BirthDate = customerRegistrationRequest.Customer.BirthDate,
                };

                // maybe add to the repositoryHandler with .Add() method?
                this.repositoryHandler.UserRepository.Add(user);

                // maybe complete transaction here?
                await this.repositoryHandler.ComleteAsync();
                var mappedCustomer = this.mapper.Map<CustomerModel>(customer);


                var token = await Authentication(new AuthenticationRequestModel
                {
                    UserName = customerRegistrationRequest.Customer.User.UserName,
                    Password = customerRegistrationRequest.Customer.User.Password
                });

                return new CustomerRegistrationResponse
                {
                    Customer = mappedCustomer,
                    Token = token.Token,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (RegistrationException ex)
            {
                // maybe RollBack on the upper mentioned using(var something = this.repositoryHandler.CreateTransactionAsync()
                await transaction.RollbackAsync();
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<MerchantRegistrationResponse> MerchantRegistration(
            MerchantRegistrationRequest merchantRegistrationRequest)
        {
            if (merchantRegistrationRequest == null)
            {
                throw new RegistrationException("Bad request ... ");
            }

            if (!merchantRegistrationRequest.Merchant.CompanyName.IsValidName(out errorMessage))
            {
                throw new RegistrationException(errorMessage);
            }

            await using var transaction = await this.repositoryHandler.CreateTransactionAsync();
            try
            {
                var user = await this.CreateUser(merchantRegistrationRequest.Merchant.User);
                user.Type = UserType.MERCHANT;
                user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                user.Active = true;

                var merchant = new MerchantEntity
                {
                    User = user,
                    UserId = user.Id,
                    CompanyName = merchantRegistrationRequest.Merchant.CompanyName,
                };

                // maybe add to the repositoryHandler with .Add() method?
                this.repositoryHandler.UserRepository.Add(user);

                // maybe complete transaction here?
                await this.repositoryHandler.ComleteAsync();
                var mappedMerchant = this.mapper.Map<MerchantModel>(merchant);


                var token = await Authentication(new AuthenticationRequestModel
                {
                    UserName = merchantRegistrationRequest.Merchant.User.UserName,
                    Password = merchantRegistrationRequest.Merchant.User.Password
                });

                return new MerchantRegistrationResponse
                {
                    Merchant = mappedMerchant,
                    Token = token.Token,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (RegistrationException ex)
            {
                await transaction.RollbackAsync();
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<IUserAccount> GetUserById(long userId)
        {
            return await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
        }

        public async Task<IUserAccount> GetUser(string username)
        {
            return await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.UserName == username);
        }

        public async Task<bool> IsActive(string username)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.UserName == username);
            return user.ExecutionStatus == UserExecutionStatus.ACTIVE;
        }

        public async Task<bool> IsActive(long userId)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
            return user.ExecutionStatus == UserExecutionStatus.ACTIVE;
        }

        public async Task<UserExecutionStatus> GetUserExecutionStatus(string username)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.UserName == username);
            return user.ExecutionStatus;
        }

        public async Task<UserExecutionStatus> GetUserExecutionStatus(long userId)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
            return user.ExecutionStatus;
        }

        public async Task SetUserExecutionStatus(UserExecutionStatus status, string username)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.UserName == username);
            user.ExecutionStatus = status;

            await this.repositoryHandler.ComleteAsync();
        }

        public async Task SetUserExecutionStatus(UserExecutionStatus status, long userId)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
            user.ExecutionStatus = status;

            await this.repositoryHandler.ComleteAsync();
        }

        // Not sure about this
        public async Task<IEnumerable<IUserAccount>> GetAllUsersByType(UserType userType)
        {
            IEnumerable<IUserAccount> users;
            var allUsers = this.repositoryHandler.UserRepository.GetAllUsers();

            switch (userType)
            {
                case UserType.CUSTOMER:
                    users = await allUsers.Where(x => x.Type == userType).Include(x => x.Customer)
                        .ToListAsync();
                    break;
                case UserType.MERCHANT:
                    users = await allUsers.Where(x => x.Type == userType).Include(x => x.Merchant)
                        .ToListAsync();
                    break;
                default:
                    users = await allUsers.Include(x => x.Customer).Include(x => x.Merchant)
                        .ToListAsync();
                    break;
            }

            return users;
        }
        private async Task<bool> IsValidUserModel(UserModel user)
        {
            if (!user.UserName.IsValidUsername(out errorMessage))
            {
                return false;
            }

            // TODO: Check if there is a user with the same username
            // maybe something like await repositoryHandler.IfAny() ??
            bool userNameExists =
                await this.repositoryHandler.UserRepository.IfAnyAsync(x => x.UserName == user.UserName);
            if (userNameExists)
            {
                return false;
            }

            if (!user.Password.IsValidPassword(out errorMessage))
            {
                return false;
            }

            if (!user.EmailAddress.IsValidEmailAddress(out errorMessage))
            {
                return false;
            }

            if (!user.PhoneNumber.IsValidPhoneNumber(out errorMessage))
            {
                return false;
            }

            return true;
        }

        private string GenerateToken(List<Claim> authClaims)
        {
            var value = configuration.GetSection("AppSettings:Jwt").Value;
            if (value != null)
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                    .GetBytes(value));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(4),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }

            throw new JsonException("No JWT section in AppSettings"); 
        }
        
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber)
                .Trim('=') // Remove padding characters
                .Replace('/', '-') // Replace URL-unsafe characters
                .Replace('+', '_');
        }
        private void StoreRefreshToken(long userId, string refreshToken)
        {
            // Replace with secure storage mechanism (e.g., database)
            // This in-memory example just demonstrates the concept
            refreshTokens.TryAdd(userId, refreshToken); 
        }
    }
}