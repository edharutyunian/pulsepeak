using PulsePeak.Core.Enums;

namespace PulsePeak.Core.ViewModels.UserViewModels
{
    public class UserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}