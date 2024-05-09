using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels.AddressModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // TODO: Add and implement any new required methods
    public interface IAddressOperations
    {
        Task<AddressModel> AddCustomerBillingAddress(long customerId, AddressModel addressModel);
        Task<AddressModel> AddCustomerShippingAddress(long customerId, AddressModel addressModel);
        Task<AddressModel> AddMerchantBillingAddress(long merchantId, AddressModel addressModel);

        Task<AddressModel> EditCustomerBillingAddress(long customerId, BillingAddressInfoModel billingAddressInfo);
        Task<AddressModel> EditCustomerShippingAddress(long customerId, ShippingAddressInfoModel shippingAddressInfo);
        Task<AddressModel> EditMerchantBillingAddress(long merchantId, BillingAddressInfoModel billingAddressInfo);

        Task DeactivateAddress(long addressId);

        Task<AddressModel> GetAddress(long addressId);
    }
}