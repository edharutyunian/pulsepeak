using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.Core.ViewModels.AddressModels
{
    public class AddressModel
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public long MerchantId { get; set; }
        public MerchantModel Merchant { get; set; }

        public required string Street { get; set; }
        public string? Unit { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public required AddressType AddressType { get; set; }

        public string? LocationName { get; set; }
        public string? RecipiantName { get; set; }
        public string? DeliveryInstructions { get; set; }
    }
}
