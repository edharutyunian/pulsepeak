using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class AddressOperations : IAddressOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public AddressOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public async Task<AddressModel> AddCustomerBillingAddress(long customerId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // validate customer
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                addressModel.CustomerId = customerId;
                addressModel.Customer = this.mapper.Map<CustomerModel>(customer);

                var addedAddress = this.repositoryHandler.AddressRepository.AddCustomerAddress(customerId, addressModel);
                var billingAddress = this.mapper.Map<AddressBaseEntity>(addressModel);

                customer.BillingAddressId = billingAddress.Id;
                customer.BillingAddress = billingAddress;

                // update customer entity
                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(customer)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return addedAddress;
            }
            catch (RegistrationException ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<AddressModel> AddCustomerShippingAddress(long customerId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // validate customer
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                addressModel.CustomerId = customerId;
                addressModel.Customer = this.mapper.Map<CustomerModel>(customer);

                var addedAddress = this.repositoryHandler.AddressRepository.AddCustomerAddress(customerId, addressModel);
                var shippingAddress = this.mapper.Map<AddressBaseEntity>(addressModel);

                customer.ShippingAddressId = shippingAddress.Id;
                customer.ShippingAddress = shippingAddress;

                // update customer entity
                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(customer)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return addedAddress;
            }
            catch (RegistrationException ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task<AddressModel> AddMerchantBillingAddress(long merchantId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                if (addressModel.AddressType != AddressType.Billing) {
                    throw new RegistrationException($"Merchant cannot have a {nameof(AddressType.Shipping)} address.");
                }

                // validate customer
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{merchantId}' not found.");

                addressModel.MerchantId = merchantId;
                addressModel.Merchant = this.mapper.Map<MerchantModel>(merchant);

                var addedAddress = this.repositoryHandler.AddressRepository.AddMerchantAddress(merchantId, addressModel);
                var billingAddress = this.mapper.Map<AddressBaseEntity>(addressModel);

                merchant.BillingAddressId = billingAddress.Id;
                merchant.BillingAddress = billingAddress;

                // update merchant entity
                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(merchant)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return addedAddress;
            }
            catch (RegistrationException ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task DeactivateAddress(long addressId)
        {
            try {
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressId)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressId}' not found.");

                address.Active = false;
                var isUpdated = repositoryHandler.AddressRepository.Update(address);
                if (!isUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> EditCustomerBillingAddress(long customerId, BillingAddressInfoModel billingAddressInfo)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(this.mapper.Map<AddressModel>(billingAddressInfo))) {
                    throw new EditableArgumentException(this.errorMessage, new EditableArgumentException(this.errorMessage));
                }

                // get customer entity
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                // check if the customer's billing Id is the same as the model's 
                if (customer.BillingAddressId != billingAddressInfo.Id) {
                    throw new RegistrationException($"Provided Customer ID '{customerId}' and Billing Address ID '{billingAddressInfo.Id} are not matching.'");
                }

                // get the address entity
                var billingAddressEntity = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == billingAddressInfo.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{billingAddressInfo.Id}' not found.");

                billingAddressEntity.CustomerId = customerId;
                billingAddressEntity.Customer = customer;

                customer.BillingAddress = billingAddressEntity;

                // update entities
                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(billingAddressEntity);
                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }

                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(CustomerEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<AddressModel>(billingAddressInfo);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> EditCustomerShippingAddress(long customerId, ShippingAddressInfoModel shippingAddressInfo)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(this.mapper.Map<AddressModel>(shippingAddressInfo))) {
                    throw new EditableArgumentException(this.errorMessage, new EditableArgumentException(this.errorMessage));
                }

                // get customer entity
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                // check if the customer's billing Id is the same as the model's 
                if (customer.BillingAddressId != shippingAddressInfo.Id) {
                    throw new RegistrationException($"Provided Customer ID '{customerId}' and Billing Address ID '{shippingAddressInfo.Id} are not matching.'");
                }

                // get the address entity
                var shippingAddressEntity = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == shippingAddressInfo.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{shippingAddressInfo.Id}' not found.");

                shippingAddressEntity.CustomerId = customerId;
                shippingAddressEntity.Customer = customer;

                customer.ShippingAddress = shippingAddressEntity;

                // update entities
                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(shippingAddressEntity);
                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }

                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(CustomerEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<AddressModel>(shippingAddressInfo);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> EditMerchantBillingAddress(long merchantId, BillingAddressInfoModel billingAddressInfo)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(this.mapper.Map<AddressModel>(billingAddressInfo))) {
                    throw new EditableArgumentException(this.errorMessage, new EditableArgumentException(this.errorMessage));
                }

                // get merchant entity
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                    ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                // check if the merchants's billing Id is the same as the model's 
                if (merchant.BillingAddressId != billingAddressInfo.Id) {
                    throw new RegistrationException($"Provided Merchant ID '{merchantId}' and Billing Address ID '{billingAddressInfo.Id} are not matching.'");
                }

                // get the address entity
                var billingAddressEntity = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == billingAddressInfo.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{billingAddressInfo.Id}' not found.");

                billingAddressEntity.MerchantId = merchantId;
                billingAddressEntity.Merchant = merchant;

                merchant.BillingAddress = billingAddressEntity;

                // update entities
                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(billingAddressEntity);
                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }

                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(MerchantEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<AddressModel>(billingAddressInfo);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> GetAddress(long addressId)
        {
            try {
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressId)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressId}' not found.");

                return this.mapper.Map<AddressModel>(address);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        // TODO: Implement... Abstract this out, need to be used in the API model as well 
        private bool IsValidAddress(AddressModel addressModel)
        {
            return true;
        }
    }
}
