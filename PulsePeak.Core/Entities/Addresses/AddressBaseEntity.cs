using System.ComponentModel.DataAnnotations;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Addresses
{
    public class AddressBaseEntity : EntityBase, IAddress
    {
        public long UserId { get; set; }
        public UserBaseEnttity User { get; set; }

        [Required]
        public string Street { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string? Unit { get; set; } // optional
        [Required]
        public string City { get; set; } // required -- validation?
        [Required]
        public string State { get; set; } // required -- validation || maybe add a public API to check this based on the country?
        [Required]
        public string ZipCode { get; set; } // required -- validation || maybe add a public API to check this based on the country and state?
        [Required]
        public string Country { get; set; } // required -- validation || maybe add a public API to check this based?
        public double Latitude { get; set; } // optional
        public double Longitude { get; set; } // optional

        // TODO: [ED] add the Type to the Models??
        public AddressType Type { get; set; } // required upon creation
        public AddressExecutionStatus AddressStatus { get; set; } // defaults to AddressExecutionStatus.NotValidated upon creation 
    }
}
