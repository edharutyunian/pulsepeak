namespace PulsePeak.Core.ViewModels.AuthModels
{
    // [Alex] Is there a need for this?
    public class AuthenticationResponseModel
    {
        public required string UserName { get; set; }
        public DateTime AuthenticationDate { get; set; } // not sure if this should be a DateTime, but it's fine I guess 
        public required string Token { get; set; }
    }
}
