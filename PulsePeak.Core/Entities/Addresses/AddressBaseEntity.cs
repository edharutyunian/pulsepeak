using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Addresses
{
    public class AddressBaseEntity : EntityBase
    {
        public string Street { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string? Unit { get; set; } // optional
        public string City { get; set; } // required -- validation?
        public string State { get; set; } // required -- validation || maybe add a public API to check this based on the country?
        public string ZipCode { get; set; } // required -- validation || maybe add a public API to check this based on the country and state?
        public string Country { get; set; } // required -- validation || maybe add a public API to check this based?
        public double Latitude { get; set; } // optional
        public double Longitude { get; set; } // optional
        public IUserAccount Owner { get; set; } // required upon creation
        public AddressExecutionStatus AddressStatus { get; set; } // defaults to AddressExecutionStatus.NotValidated upon creation 
    }
}
