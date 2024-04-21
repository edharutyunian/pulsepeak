namespace PulsePeak.Core.ViewModels.UserViewModels
{
    public class UserModel
    {
        public long Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }
    }
}