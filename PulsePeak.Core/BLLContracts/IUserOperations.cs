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
        Task<IUserAccount> GetUserById(long userId);
        Task<IEnumerable<IUserAccount>> UserList(UserType userType);
        Task<UserExecutionStatus> IsActive(string username);
        Task SetUserStatus(UserExecutionStatus status, string username);

        // maybe ResetPassword() and ChangePassword() ??
    }
}