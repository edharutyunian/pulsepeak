﻿namespace PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels
{
    public class MerchantRegistrationResponse
    {
        public MerchantModel Merchant { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
