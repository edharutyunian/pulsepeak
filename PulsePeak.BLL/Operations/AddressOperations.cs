using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Utils.EntityHelpers;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class AddressOperations : IAddressOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private readonly string errorMessage;

        public AddressOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        // TODO [Ed]: Refactor
        public async Task<AddressModel> AddAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: validate model and move to the api layer
            try {
                var addedAddress = this.repositoryHandler.AddressRepository.AddAddress(userId, addressModel);
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

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

        // TODO [Ed]: Refactor
        public async Task<AddressModel> EditBillingAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: Validate model
            try {
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");

                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                var updatedAddress = this.mapper.Map<AddressBaseEntity>(addressModel);

                // TODO [ED]: ask Tigran, Davit |  not sure on this; the MappingProfile needs to be configured properly to consider the User.Type
                var updatedUser = this.mapper.Map<UserBaseEnttity>(addressModel.User);


                var isAddressUpdated = this.repositoryHandler.AddressRepository.Update(updatedAddress);
                var isUserAddressUpdated = this.repositoryHandler.UserRepository.Update(updatedUser);

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
                throw new EditableArgumentException(ex.Message, ex);
            }
        }

        // TODO [Ed]: Refactor
        public async Task<AddressModel> EditShippingAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: Validate model
            try {
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"User with ID '{userId}' not found.");

                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");


                // TODO [ED]: ask Tigran, Davit |  not sure on this; the MappingProfile needs to be configured properly to consider the User.Type

                bool isAddressUpdated = address.StartUpdatingProperties()
                                                .UpdateProperty(x => x.Street, addressModel.Street)
                                                .UpdateProperty(x => x.Unit, addressModel.Unit)
                                                .UpdateProperty(x => x.City, addressModel.City)
                                                .UpdateProperty(x => x.State, addressModel.State)
                                                .UpdateProperty(x => x.Country, addressModel.Country)
                                                .UpdateProperty(x => x.ZipCode, addressModel.ZipCode)
                                                .UpdateProperty(x => x.AddressType, addressModel.AddressType)
                                                .IsPropertyUpdated;

                bool isUserAddressUpdated = user.Type == UserType.CUSTOMER
                                            ? user.StartUpdatingProperties()
                                                .UpdateProperty(x => x.Customer.ShippingAddress, address)
                                                .IsPropertyUpdated
                                            : false;

                if (isAddressUpdated && isUserAddressUpdated) {
                    await this.repositoryHandler.SaveAsync();
                }

                return addressModel;
            }
            catch (EditableArgumentException ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new EditableArgumentException(ex.Message, ex);
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

        public async Task<AddressModel> GetAddress(long userId, AddressType addressType)
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

        // TODO [Ed]: Refactor
        public async Task<bool> SetShippingAddress(long userId, AddressModel addressModel)
        {
            try {
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId)
                    ?? throw new EntityNotFoundException($"Address with ID '{userId}' not found.");

                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id)
                    ?? throw new EntityNotFoundException($"Address with ID '{addressModel.Id}' not found.");

                user.Customer.ShippingAddress = address;

                var isAddressUpdated = repositoryHandler.AddressRepository.Update(address);
                var isUserUpdated = repositoryHandler.UserRepository.Update(user);

                if (!isAddressUpdated) {
                    throw new DbContextException($"The {nameof(AddressBaseEntity)} has not been updated.");
                }
                else if (!isUserUpdated) {
                    throw new DbContextException($"The {nameof(UserBaseEnttity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        private bool IsValidAddress(AddressModel addressModel)
        {
            // TODO: implement
            return true;
        }
    }
}
