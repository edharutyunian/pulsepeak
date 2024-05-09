using PulsePeak.Core.ViewModels.AuthModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // Deprecated => Just left here for the auth stuff
    public interface IUserOperations
    {
        Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest);
        Task<AuthResponse> VerifyAndGenerateToken(TokenRequest tokenRequest);
    }
}