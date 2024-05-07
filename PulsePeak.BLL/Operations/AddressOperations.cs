using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.BLLOperationContracts;
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

        public async Task<AddressModel> AddAddress(long userId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // check user existence
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                if (addressModel.User.Id != user.Id) {
                    throw new RegistrationException($"Model's User ID '{addressModel.User.Id}' and provided User ID '{user.Id}' are not matching.");
                }

                // add the address
                var addedAddress = this.repositoryHandler.AddressRepository.AddAddress(userId, addressModel);

                // update user entity
                var isUserUpdated = this.repositoryHandler.UserRepository.Update(user);
                if (!isUserUpdated) {
                    throw new DbContextException($"The {nameof(user)} has not been updated.");
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

        public async Task<AddressModel> EditBillingAddress(long userId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new EditableArgumentException(this.errorMessage, new EditableArgumentException(this.errorMessage));
                }

                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                if (addressModel.User.Id != user.Id) {
                    throw new RegistrationException($"Model's User ID '{addressModel.User.Id}' and provided User ID '{user.Id}' are not matching.");
                }

                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");

                var mappedAddress = this.mapper.Map<AddressBaseEntity>(addressModel);
                var mappedUser = this.mapper.Map<UserBaseEnttity>(addressModel.User);

                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(mappedAddress);
                var isUserAddressUpdated = this.repositoryHandler.UserRepository.Update(mappedUser);

                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }
                if (!isUserAddressUpdated) {
                    throw new DbContextException($"The {nameof(UserBaseEnttity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();
                return addressModel;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> EditShippingAddress(long userId, AddressModel addressModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidAddress(addressModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                if (user.Type != UserType.CUSTOMER) {
                    throw new EditableArgumentException($"The {nameof(user.Type)} is not acceptable for editing.");
                }

                if (addressModel.User.Id != user.Id) {
                    throw new RegistrationException($"Model's User ID '{addressModel.User.Id}' and provided User ID '{user.Id}' are not matching.");
                }

                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");

                var mappedAddress = this.mapper.Map<AddressBaseEntity>(addressModel);
                var mappedUser = this.mapper.Map<UserBaseEnttity>(addressModel.User);

                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(mappedAddress);
                var isUserAddressUpdated = this.repositoryHandler.UserRepository.Update(mappedUser);

                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }
                if (!isUserAddressUpdated) {
                    throw new DbContextException($"The {nameof(UserBaseEnttity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();
                return addressModel;
            }
            catch (EditableArgumentException ex) {
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

        public async Task<AddressModel> GetUsersAddresses(long userId, AddressType addressType)
        {
            try {
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.UserId == userId && x.AddressType == addressType)
                        ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                return this.mapper.Map<AddressModel>(address);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetShippingAddress(long userId, AddressModel addressModel)
        {
            try {
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"Address with ID '{userId}' not found.");

                if (user.Type != UserType.CUSTOMER) {
                    throw new EditableArgumentException($"The {nameof(user.Type)} is not acceptable.");
                }

                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");

                user.Customer.ShippingAddress = address;

                var isAddressUpdated = repositoryHandler.AddressRepository.Update(address);
                var isUserUpdated = repositoryHandler.UserRepository.Update(user);

                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }
                if (!isUserUpdated) {
                    throw new DbContextException($"The {nameof(UserBaseEnttity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
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
