using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.AuthModels;
using PulsePeak.Core.ViewModels.UserViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.Core.BLLContracts
{
    public interface IUserOperations
    {
        Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest);
        Task<AuthResponse> VerifyAndGenerateToken(TokenRequest tokenRequest);
        Task<UserBaseEnttity> CreateUser(UserModel userModel);
        Task<CustomerRegistrationResponse> CustomerRegistration(CustomerRegistrationRequest customerRegistrationRequest);
        Task<MerchantRegistrationResponse> MerchantRegistration(MerchantRegistrationRequest merchantRegistrationRequest);
        Task<IUserAccount> GetUser(long userId);
        Task<IUserAccount> GetUser(string username);
        Task<IEnumerable<IUserAccount>> GetAllUsersByType(UserType userType);
        Task<bool> IsActive(string username);
        Task<bool> IsActive(long userId);
        Task<UserExecutionStatus> GetUserExecutionStatus(string username);
        Task<UserExecutionStatus> GetUserExecutionStatus(long userId);
        Task SetUserExecutionStatus(UserExecutionStatus status, string username);
        Task SetUserExecutionStatus(UserExecutionStatus status, long userId);

        // maybe ResetPassword() and ChangePassword() ??
    }
}