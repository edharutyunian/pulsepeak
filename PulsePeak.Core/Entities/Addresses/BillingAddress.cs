using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Addresses
{
    public class BillingAddress : AddressBaseEntity, IAddress
    {
        public long UserId { get; set; }
        public UserBaseEnttity User { get; set; }

        public string? BillToName { get; set; } // optional -- defaults to User FullName
        public bool IsTaxable { get; set; } // default to false || not required for ARM users 
    }
}
