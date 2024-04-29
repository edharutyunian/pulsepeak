namespace PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels
{
    public class CustomerRegistrationResponseModel
    {
        public CustomerModel Customer { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
