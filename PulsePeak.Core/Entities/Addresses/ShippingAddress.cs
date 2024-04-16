using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Addresses
{
    public class ShippingAddress : AddressBaseEntity, IAddress
    {
        public long UserId { get; set; }
        public UserBaseEnttity User { get; set; }

        public string? LocationName { get; set; } // optional => defaults to ((FirstName + " " LastName) || CompanyName) + " " Street | validation less than 50 chars
        public string? RecipientName { get; set; } // optional -- but defaults to the Users FullName if not applied 
        public string? DeliveryInstructions { get; set; } // optional
    }
}