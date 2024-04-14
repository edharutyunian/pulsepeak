namespace PulsePeak.Core.Entities.Addresses
{
    public class BillingAddress : AddressBaseEntity, IAddress
    {
        public string BillToName { get; set; } // required -- defaults to User FullName
        public bool IsTaxable { get; set; } // default to false || not required for ARM users 
    }
}
