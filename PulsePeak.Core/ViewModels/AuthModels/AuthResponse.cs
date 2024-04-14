using PulsePeak.Core.ViewModels.UserViewModels;
using System.ComponentModel.DataAnnotations;

namespace PulsePeak.Core.ViewModels.AuthModels
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
