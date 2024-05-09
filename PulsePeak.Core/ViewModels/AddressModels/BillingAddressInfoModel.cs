using PulsePeak.Core.Enums;

namespace PulsePeak.Core.ViewModels.AddressModels
{
    public class BillingAddressInfoModel
    {
        public required long Id { get; set; }
        public required string Street { get; set; }
        public string? Unit { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public AddressExecutionStatus AddressExecutionStatus { get; set; }
        public AddressType AddressType => AddressType.Billing;
    }
}
