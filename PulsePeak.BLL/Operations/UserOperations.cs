using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLContracts;
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
        // need to add something like TokenKey and TokenParameters or so for the Auth
        private string errorMessage = string.Empty;

        public UserOperations(IServiceProvider serviceProvider)
        {
            this.repositoryHandler = serviceProvider.GetRequiredService<IRepositoryHandler>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest)
        {
            // Alex this is something for you to take care of
            throw new NotImplementedException();
        }

        public async Task<UserBaseEnttity> CreateUser(UserModel userModel)
        {
            if (!await IsValidUser(userModel)) {
                throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
            }

            try {
                var user = mapper.Map<UserBaseEnttity>(userModel);
                repositoryHandler.UserRepository.Add(user);
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
            if (!customerRegistrationRequest.Customer.FirstName.IsValidName(out errorMessage)
                || string.IsNullOrEmpty(customerRegistrationRequest.Customer.FirstName)) {
                throw new RegistrationException(errorMessage);
            }
            if (!customerRegistrationRequest.Customer.LastName.IsValidName(out errorMessage)
                || string.IsNullOrEmpty(customerRegistrationRequest.Customer.LastName)) {
                throw new RegistrationException(errorMessage);
            }
            if (!Validator.IsValidBirthDate(customerRegistrationRequest.Customer.BirthDate, out errorMessage)) {
                throw new RegistrationException(errorMessage);
            }

            // TODO: Implement the stuff for creating the customer
            // need something like CreateTransaction() or so for the IRepositoryhandler
            // maybe using(var something = this.repositoryHandler.CreateTransaction() { }
            // then
            try {
                // not sure on this tbh
                var user = await this.CreateUser(customerRegistrationRequest.Customer.User);
                user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                user.Type = UserType.CUSTOMER;

                var customer = new CustomerEntity {
                    UsertId = user.Id,
                    User = user,
                    FirstName = customerRegistrationRequest.Customer.FirstName,
                    LastName = customerRegistrationRequest.Customer.LastName,
                    BirthDate = customerRegistrationRequest.Customer.BirthDate,
                };

                // maybe add to the repositoryHandler with .Add() method?
                repositoryHandler.UserRepository.Add(user);

                var mappedCustomer = mapper.Map<CustomerModel>(customer);

                // TODO: Get the token:
                // Alex -- this is something for you
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
                // maybe RollBack on the upper mentioned using(var something = this.repositoryHandler.CreateTransaction()
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public Task<IUserAccount> GetUserById(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserExecutionStatus> IsActive(string username)
        {
            throw new NotImplementedException();
        }

        public Task<MerchantRegistrationResponse> MerchantRegistration(MerchantRegistrationRequest merchantRegistrationRequest)
        {
            throw new NotImplementedException();
        }

        public Task SetUserStatus(UserExecutionStatus status, string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IUserAccount>> UserList(UserType userType)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            throw new NotImplementedException();
        }



        private async Task<bool> IsValidUser(UserModel user)
        {
            if (!user.UserName.IsValidUsername(out errorMessage)) {
                return false;
            }

            // TODO: Check if there is a user with the same username
            // maybe something like await repositoryHandler.IfAny() ??

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
    }
}