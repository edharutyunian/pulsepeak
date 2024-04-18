using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Addresses
{
    public class BillingAddress : EntityBase, IAddress
    {
        public long AddressId { get; set; }
        public AddressBaseEntity AddressBase { get; set; }

        public long UserId { get; set; }
        public UserBaseEnttity User { get; set; }

        public string? BillToName { get; set; } // optional -- defaults to User FullName
        public bool IsTaxable { get; set; } // default to false || not required for ARM users 
    }
}
