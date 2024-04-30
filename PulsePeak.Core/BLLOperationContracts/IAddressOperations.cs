using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // TODO: Add and implement any new required methods
    public interface IAddressOperations
    {
        Task<AddressModel> AddAddress(long userId, AddressModel addressModel);
        Task<AddressModel> EditBillingAddress(long userId, AddressModel addressModel);
        Task<AddressModel> EditShippingAddress(long userId, AddressModel addressModel);
        Task DeactivateAddress(long addressId);

        Task<bool> SetShippingAddress(long userId, AddressModel addressModel);

        Task<AddressModel> GetAddress(long addressId);
        Task<AddressModel> GetAddress(long userId, AddressType addressType);
    }
}