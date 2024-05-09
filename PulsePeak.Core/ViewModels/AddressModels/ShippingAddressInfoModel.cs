using PulsePeak.Core.Enums;

namespace PulsePeak.Core.ViewModels.AddressModels
{
    public class ShippingAddressInfoModel
    {
        public required long Id { get; set; }
        public required string Street { get; set; }
        public string? Unit { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public AddressExecutionStatus AddressExecutionStatus { get; set; }
        public AddressType AddressType => AddressType.Shipping;

        public string? LocationName { get; set; }
        public string? RecipiantName { get; set; }
        public string? DeliveryInstructions { get; set; }
    }
}
