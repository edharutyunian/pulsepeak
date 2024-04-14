namespace PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels
{
    public class CustomerRegistrationResponse
    {
        public CustomerModel Customer { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
