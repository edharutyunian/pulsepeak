namespace PulsePeak.Core.Entities.Addresses
{
    public class ShippingAddress : AddressBaseEntity, IAddress
    {
        public string LocationName { get; set; } // required -- validation less than 50 chars
        public string? RecipientName { get; set; } // optional -- but defaults to the Users FullName if not applied 
        public string? DeliveryInstructions { get; set; } // optional
    }
}