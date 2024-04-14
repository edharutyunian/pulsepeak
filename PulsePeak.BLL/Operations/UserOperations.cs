using PulsePeak.Core.BLLContracts;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryAbstraction;
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
        // need to add something like TokenKey and TokenParameters or so for the Auth
        private string errorMessage = string.Empty;

        public UserOperations(IRepositoryHandler repositoryHandler)
        {
            this.repositoryHandler = repositoryHandler;
        }

        public Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest)
        {
            // Alex this is something for you to take care of
            throw new NotImplementedException();
        }

        public Task<UserBaseEnttity> CreateUser(UserModel userModel)
        {
            throw new NotImplementedException();
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
                var user = await ((IUserOperations) this).CreateUser(customerRegistrationRequest.Customer.User);
                user.ExecutionStatus = UserExecutionStatus.NOTVERIFIED;
                user.Type = UserType.CUSTOMER;

                var customer = new Customer {
                    FirstName = customerRegistrationRequest.Customer.FirstName,
                    LastName = customerRegistrationRequest.Customer.LastName,
                    UserName = customerRegistrationRequest.Customer.User.UserName,
                    Password = customerRegistrationRequest.Customer.User.Password,
                    BirthDate = customerRegistrationRequest.Customer.BirthDate,
                };

                // maybe add to the repositoryHandler with .Add() method?

                // this is used to avoid errors :
                return new CustomerRegistrationResponse();

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
    }
}