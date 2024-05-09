using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Utils.Validators;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Utils.Extensions;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.ViewModels.AuthModels;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class CustomerOperations : ICustomerOperations
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IUserOperations userOperations; // [ALEX] not sure on this; probabaly can be something like IAuthOperatios or so; just a tweak rn
        private string errorMessage;

        public CustomerOperations(ILogger logger, IMapper mapper, IRepositoryHandler repositoryHandler, IUserOperations userOperations)
        {
            this.log = logger;
            this.mapper = mapper;
            this.repositoryHandler = repositoryHandler;
            this.userOperations = userOperations;
            this.errorMessage = string.Empty;
        }

        public async Task<CustomerRegistrationResponseModel> CustomerRegistration(CustomerRegistrationRequestModel customerRegistrationRequest)
        {
            try {
                // TODO: Move the validation to the API layer
                if (!await IsValidCustomerModel(customerRegistrationRequest.Customer)) {
                    throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
                }

                // TODO: can also be checked in the API layer, even UI; this is just a tweak here
                if (!Validator.IsValidBirthDate(customerRegistrationRequest.Customer.BirthDate, out errorMessage)) {
                    throw new RegistrationException(errorMessage);
                }

                var customer = this.mapper.Map<CustomerEntity>(customerRegistrationRequest.Customer);
                customer.Active = true;
                customer.ExecutionStatus = UserExecutionStatus.ACTIVE;
                customer.IsActive = true;

                var addedCustomer = this.repositoryHandler.CustomerRepository.Add(customer);

                // TODO [Alex]: Get the token -- this is something for you
                var token = await this.userOperations.Authentication(new AuthenticationRequestModel {
                    UserName = customerRegistrationRequest.Customer.UserName,
                    Password = customerRegistrationRequest.Customer.Password
                });

                return new CustomerRegistrationResponseModel {
                    Customer = this.mapper.Map<CustomerModel>(addedCustomer),
                    Token = token.Token,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new RegistrationException(errorMessage, ex);
            }
        }

        // TODO [ED]: Implement
        public Task<CustomerModel> EditCustomerInfo(CustomerModel customerModel)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerModel> GetCustomer(long customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                        ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                return this.mapper.Map<CustomerModel>(customer);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<CustomerModel> GetCustomer(string username)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Customer with username '{username}' not found.");

                return this.mapper.Map<CustomerModel>(customer);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> GetCustomerBillingAddress(int customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                var billingAddress = customer.BillingAddress;

                return this.mapper.Map<AddressModel>(billingAddress);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<UserExecutionStatus> GetCustomerExecutionStatus(long customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                        ?? throw new EntityNotFoundException($"User with ID '{customerId}' not found.");

                return customer.ExecutionStatus;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<UserExecutionStatus> GetCustomerExecutionStatus(string username)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Customer with username '{username}' not found.");

                return customer.ExecutionStatus;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> GetCustomerShippingAddress(int customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                var shippingAddress = customer.ShippingAddress;

                return this.mapper.Map<AddressModel>(shippingAddress);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> IsActive(long customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                return customer.ExecutionStatus == UserExecutionStatus.ACTIVE;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> IsActive(string username)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.UserName == username)
                    ?? throw new EntityNotFoundException($"Customer with username '{username}' not found.");

                return customer.ExecutionStatus == UserExecutionStatus.ACTIVE;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task SetCustomerExecutionStatus(long customerId, UserExecutionStatus status)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer  with ID '{customerId}' not found.");

                customer.ExecutionStatus = status;

                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(customer)} has not been updated.");
                }
                await this.repositoryHandler.SaveAsync();
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task SetCustomerExecutionStatus(string username, UserExecutionStatus status)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.UserName == username)
                    ?? throw new EntityNotFoundException($"Customer  with username '{username}' not found.");

                customer.ExecutionStatus = status;

                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(customer)} has not been updated.");
                }
                await this.repositoryHandler.SaveAsync();
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        // TODO [ED]: abstract this out, need to be used in the API model as well 
        private async Task<bool> IsValidCustomerModel(CustomerModel customermodel)
        {
            if (!customermodel.UserName.IsValidUsername(out errorMessage)) {
                return false;
            }
            // TODO: Check if there is a user with the same username
            // maybe something like await repositoryHandler.IfAny() ??
            bool userNameExists = await this.repositoryHandler.CustomerRepository.IfAnyAsync(x => x.UserName == customermodel.UserName);
            if (userNameExists) {
                return false;
            }

            if (!customermodel.Password.IsValidPassword(out errorMessage)) {
                return false;
            }
            if (!customermodel.EmailAddress.IsValidEmailAddress(out errorMessage)) {
                return false;
            }
            if (!customermodel.PhoneNumber.IsValidPhoneNumber(out errorMessage)) {
                return false;
            }
            return true;
        }
    }
}