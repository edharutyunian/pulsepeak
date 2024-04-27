using AutoMapper;
using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.BLLContracts;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Utils.EntityHelpers;

namespace PulsePeak.BLL.Operations
{
    public class AddressOperations : IAddressOperations
    {
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private readonly string errorMessage;

        public AddressOperations(IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public async Task<AddressModel> AddAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: validate model and move to the api layer
            try {
                var addedAddress = this.repositoryHandler.AddressRepository.AddAddress(userId, addressModel);
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);

                string locationName = addedAddress.AddressType == AddressType.Shipping
                    ? $"{user.FirstName} {user.LastName} - {addedAddress.Street}"
                    : "";
                string recipiantName = $"{user.FirstName} {user.LastName}";

                // TODO [ED]: use the mapper here
                var address = new AddressBaseEntity {
                    UserId = userId,
                    User = user,
                    Id = addedAddress.Id,
                    Street = addedAddress.Street,
                    Unit = addedAddress.Unit,
                    City = addedAddress.City,
                    State = addedAddress.State,
                    Country = addedAddress.Country,
                    ZipCode = addedAddress.ZipCode,
                    AddressType = addressModel.AddressType,
                    LocationName = locationName,
                    RecipientName = recipiantName,
                };

                this.repositoryHandler.AddressRepository.Add(address);
                await this.repositoryHandler.SaveAsync();

                return new AddressModel {
                    Id = address.Id,
                    AddressType = address.AddressType,
                    Street = address.Street,
                    Unit = address.Unit,
                    City = address.City,
                    State = address.State,
                    Country = address.Country,
                    ZipCode = address.ZipCode,
                };
            }
            catch (RegistrationException ex) {
                throw new RegistrationException(ex.Message, ex);
            }
        }

        public async Task DeactivateAddress(long addressId)
        {
            var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressId);
            address.Active = false;
            repositoryHandler.AddressRepository.Update(address);
        }

        public async Task<AddressModel> EditBillingAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: Validate model

            try {
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id);

                if (address == null || user == null) {
                    throw new EditableArgumentException(nameof(address));
                }

                // TODO [ED]: use the mapper here 
                #region don't like updating this way
                //address.User = user;
                //address.Street = addressModel.Street;
                //address.Unit = addressModel.Unit;
                //address.City = addressModel.City;
                //address.State = addressModel.State;
                //address.Country = addressModel.Country;
                //address.ZipCode = addressModel.ZipCode;

                //// update customer's billing address
                //if (user.Type == UserType.CUSTOMER) {
                //    address.RecipientName = $"{user.Customer.FirstName} {user.Customer.LastName}";
                //    user.Customer.BillingAddress = address;
                //    await this.repositoryHandler.SaveAsync();
                //}
                //// update customer's billing address
                //else if (user.Type == UserType.MERCHANT) {
                //    address.LocationName = $"{user.Merchant.FirstName} {user.Merchant.LastName} - {address.Street}";
                //    user.Merchant.BillingAddress = address;
                //    await this.repositoryHandler.SaveAsync();
                //}
                #endregion
                // Maybe something like this??? 
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
                                                .UpdateProperty(x => x.Customer.BillingAddress, address)
                                                .IsPropertyUpdated
                                            : user.Type == UserType.MERCHANT ?
                                                user.StartUpdatingProperties()
                                                    .UpdateProperty(x => x.Merchant.BillingAddress, address)
                                                    .IsPropertyUpdated
                                            : false;

                var newAddress = this.mapper.Map<AddressBaseEntity>(addressModel);


                if (isAddressUpdated && isUserAddressUpdated) {
                    await this.repositoryHandler.SaveAsync();
                }

                return addressModel;
            }
            catch (EditableArgumentException ex) {
                throw new EditableArgumentException(ex.Message, ex);
            }
        }

        public async Task<AddressModel> EditShippingAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: Validate model

            try {
                var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
                var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id);

                if (address == null || user == null) {
                    throw new EditableArgumentException(nameof(address));
                }

                // TODO [ED]: use the mapper here
                #region don't like updating this way
                //address.User = user;
                //address.Street = addressModel.Street;
                //address.Unit = addressModel.Unit;
                //address.City = addressModel.City;
                //address.State = addressModel.State;
                //address.Country = addressModel.Country;
                //address.ZipCode = addressModel.ZipCode;

                //// update customer's billing address
                //if (user.AddressType == UserType.CUSTOMER) {
                //    address.RecipientName = $"{user.Customer.FirstName} {user.Customer.LastName}";
                //    user.Customer.BillingAddress = address;
                //    await this.repositoryHandler.SaveAsync();
                //}
                //// update customer's billing address
                //else if (user.AddressType == UserType.MERCHANT) {
                //    address.LocationName = $"{user.Merchant.FirstName} {user.Merchant.LastName} - {address.Street}";
                //    user.Merchant.BillingAddress = address;
                //    await this.repositoryHandler.SaveAsync();
                //}
                #endregion
                // Maybe something like this??? 
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
                throw new EditableArgumentException(ex.Message, ex);
            }
        }

        public async Task<AddressModel> GetAddress(long addressId)
        {
            // TODO [ED]: add exception handling
            var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressId);
            return new AddressModel {
                Id = addressId,
                Street = address.Street,
                Unit = address.Unit,
                City = address.City,
                State = address.State,
                Country = address.Country,
                ZipCode = address.ZipCode,
                AddressType = address.AddressType,
                User = address.User
            };
        }

        public async Task<AddressModel> GetAddress(long userId, AddressType addressType)
        {
            // TODO [ED]: add exception handling
            var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.UserId == userId && x.AddressType == addressType);
            return new AddressModel {
                Id = address.Id,
                Street = address.Street,
                Unit = address.Unit,
                City = address.City,
                State = address.State,
                Country = address.Country,
                ZipCode = address.ZipCode,
                AddressType = address.AddressType,
            };
        }

        public async Task SetShippingAddress(long userId, AddressModel addressModel)
        {
            // TODO [ED]: add exception handling
            var user = await this.repositoryHandler.UserRepository.GetSingleAsync(x => x.Id == userId);
            var address = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == addressModel.Id);

            user.Customer.ShippingAddress = address;
            repositoryHandler.AddressRepository.Update(address);
            repositoryHandler.UserRepository.Update(user);
        }

        private bool IsValidAddress(AddressModel addressModel)
        {
            // TODO: implement
            return true;
        }
    }
}
