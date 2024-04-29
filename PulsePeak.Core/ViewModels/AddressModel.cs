using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.ViewModels
{
    public class AddressModel
    {
        public long Id { get; set; }
        public required string Street { get; set; }
        public string? Unit { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public required AddressType AddressType { get; set; }

        public string? LocationName { get; set; }
        public string? RecipiantName { get; set; }

        public IUserAccount User { get; set; }
    }
}
