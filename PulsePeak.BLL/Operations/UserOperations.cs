using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLContracts;
using PulsePeak.Core.Entities.Contacts;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.Utils.Extensions;
using PulsePeak.Core.Utils.Validators;
using PulsePeak.Core.ViewModels.AuthModels;
using PulsePeak.Core.ViewModels.UserViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.BLL.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        // need to add something like TokenKey and TokenParameters or so for the Auth
        private string errorMessage = string.Empty;

        public UserOperations(IServiceProvider serviceProvider)
        {
            this.repositoryHandler = serviceProvider.GetRequiredService<IRepositoryHandler>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest)
        {
            // TODO [Alex]: this is something for you to take care of
            throw new NotImplementedException();
        }

        async Task<UserBaseEnttity> IUserOperations.CreateUser(UserModel userModel)
        {
            // TODO: [ED] Move the validation to the API layer
            if (!await IsValidUserModel(userModel)) {
                throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
            }

            try {
                var user = this.mapper.Map<UserBaseEnttity>(userModel);
                user.Contacts.Add(new ContactBaseEntity {
                    UserId = user.Id,
                    User = user,
                    Active = true,
                    Type = Core.Enums.ContactType.EmailAddress,
                    Value = userModel.EmailAddress
                });
                user.Contacts.Add(new ContactBaseEntity {
                    UserId = user.Id,
                    User = user,
                    Active = true,
                    Type = Core.Enums.ContactType.PhoneNumber,
                    Value = userModel.PhoneNumber
                });
                this.repositoryHandler.UserRepository.Add(user);
                return user;
            }
            catch (RegistrationException e) {
                throw new RegistrationException(errorMessage, e);
            }
        }

        public async Task<CustomerRegistrationResponse> CustomerRegistration(CustomerRegistrationRequest customerRegistrationRequest)
        {

            if (customerRegistrationRequest == null) {
                throw new RegistrationException("Bad Request. "); // TODO: change the exception message
            }
            if (!customerRegistrationRequest.Customer.FirstName.IsValidName(out errorMessage) ||
                !customerRegistrationRequest.Customer.LastName.IsValidName(out errorMessage)) {
                throw new RegistrationException(errorMessage);
            }
            if (!Validator.IsValidBirthDate(customerRegistrationRequest.Customer.BirthDate, out errorMessage)) {
                throw new RegistrationException(errorMessage);
            }

            using (var transaction = await this.repositoryHandler.CreateTransactionAsync()) {
                try {
                    // not sure on this tbh
                    var user = await ((IUserOperations) this).CreateUser(customerRegistrationRequest.Customer.User);
                    user.Type = UserType.CUSTOMER;
                    user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                    user.Active = true;

                    var customer = new CustomerEntity {
                        UsertId = user.Id,
                        User = user,
                        FirstName = customerRegistrationRequest.Customer.FirstName,
                        LastName = customerRegistrationRequest.Customer.LastName,
                        BirthDate = customerRegistrationRequest.Customer.BirthDate,
                    };

                    // maybe add to the repositoryHandler with .Add() method?
                    this.repositoryHandler.UserRepository.Add(user);

                    // maybe complete transaction here?
                    await this.repositoryHandler.SaveAsync();
                    var mappedCustomer = this.mapper.Map<CustomerModel>(customer);

                    // TODO [Alex]: Get the token -- this is something for you
                    var token = await Authentication(new AuthenticationRequestModel {
                        UserName = customerRegistrationRequest.Customer.User.UserName,
                        Password = customerRegistrationRequest.Customer.User.Password
                    });

                    return new CustomerRegistrationResponse {
                        Customer = mappedCustomer,
                        Token = token.Token,
                        RefreshToken = token.RefreshToken
                    };

                }
                catch (RegistrationException ex) {
                    // maybe RollBack on the upper mentioned using(var something = this.repositoryHandler.CreateTransactionAsync()
                    await transaction.RollbackAsync();
                    throw new RegistrationException(ex.Message, ex);
                }
            }
        }

        public async Task<MerchantRegistrationResponse> MerchantRegistration(MerchantRegistrationRequest merchantRegistrationRequest)
        {
            if (merchantRegistrationRequest == null) {
                throw new RegistrationException("Bad request ... ");
            }
            if (!merchantRegistrationRequest.Merchant.CompanyName.IsValidName(out errorMessage)) {
                throw new RegistrationException(errorMessage);
            }

            using (var transaction = await this.repositoryHandler.CreateTransactionAsync()) {
                try {
                    var user = await ((IUserOperations) this).CreateUser(merchantRegistrationRequest.Merchant.User);
                    user.Type = UserType.MERCHANT;
                    user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                    user.Active = true;

                    var merchant = new MerchantEntity {
                        User = user,
                        UserId = user.Id,
                        CompanyName = merchantRegistrationRequest.Merchant.CompanyName,
                    };

                    // maybe add to the repositoryHandler with .Add() method?
                    this.repositoryHandler.UserRepository.Add(user);

                    // maybe complete transaction here?
                    await this.repositoryHandler.SaveAsync();
                    var mappedMerchant = this.mapper.Map<MerchantModel>(merchant);

                    // TODO [Alex]: Get the token -- this is something for you
                    var token = await Authentication(new AuthenticationRequestModel {
                        UserName = merchantRegistrationRequest.Merchant.User.UserName,
                        Password = merchantRegistrationRequest.Merchant.User.Password
                    });

                    return new MerchantRegistrationResponse {
                        Merchant = mappedMerchant,
                        Token = token.Token,
                        RefreshToken = token.RefreshToken
                    };
                }
                catch (RegistrationException ex) {
                    await transaction.RollbackAsync();
                    throw new RegistrationException(ex.Message, ex);
                }
            }
        }

        public async Task<IUserAccount> GetUser(long userId)
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

            await this.repositoryHandler.SaveAsync();
        }
        public async Task SetUserExecutionStatus(UserExecutionStatus status, long userId)
        {
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
            user.ExecutionStatus = status;

            await this.repositoryHandler.SaveAsync();
        }

        // Not sure about this
        public async Task<IEnumerable<IUserAccount>> GetAllUsersByType(UserType userType)
        {
            IEnumerable<IUserAccount> users;
            var allUsers = this.repositoryHandler.UserRepository.GetAllUsers();

            switch (userType) {
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

        public Task<AuthResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            // TODO [Alex]: take a look into this 
            throw new NotImplementedException();
        }


        private async Task<bool> IsValidUserModel(UserModel user)
        {
            if (!user.UserName.IsValidUsername(out errorMessage)) {
                return false;
            }
            // TODO: Check if there is a user with the same username
            // maybe something like await repositoryHandler.IfAny() ??
            bool userNameExists = await this.repositoryHandler.UserRepository.IfAnyAsync(x => x.UserName == user.UserName);
            if (userNameExists) {
                return false;
            }

            if (!user.Password.IsValidPassword(out errorMessage)) {
                return false;
            }
            if (!user.EmailAddress.IsValidEmailAddress(out errorMessage)) {
                return false;
            }
            if (!user.PhoneNumber.IsValidPhoneNumber(out errorMessage)) {
                return false;
            }
            return true;
        }

        private AuthResponse CreateToken(UserBaseEnttity user)
        {
            // TODO [Alex]: take a look into this

            return new AuthResponse {
                Success = true,
                Token = "",
                RefreshToken = "",
                UserInfo = new UserInfoModel {
                    EmailAddress = "",
                    FullName = ""
                }
            };
        }
    }
}