using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    public interface IMerchantOperations
    {
        Task<MerchantRegistrationResponseModel> MerchantRegistration(MerchantRegistrationRequestModel merchantRegistrationRequest);
        Task<MerchantModel> EditMerchantInfo(MerchantModel merchantModel);
        Task<MerchantModel> GetMerchant(long merchantId);
        Task<MerchantModel> GetMerchant(string username);
        Task<AddressModel> GetMerchantBillingAddress(int merchantId);
        Task<bool> IsActive(long merchantId);
        Task<bool> IsActive(string username);
        Task<UserExecutionStatus> GetMerchantExecutionStatus(long merchantId);
        Task<UserExecutionStatus> GetMerchantExecutionStatus(string username);
        Task SetMerchantExecutionStatus(long merchantId, UserExecutionStatus status);
        Task SetMerchantExecutionStatus(string username, UserExecutionStatus status);
    }
}
