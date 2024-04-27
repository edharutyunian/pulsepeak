using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Addresses
{
    [Table("Addresses")]
    public class AddressBaseEntity : EntityBase, IAddress
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UserId { get; set; }
        public required IUserAccount User { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string Street { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? Unit { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public required string City { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public required string State { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public required string ZipCode { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public required string Country { get; set; }

        public double Latitude { get; set; } // optional
        public double Longitude { get; set; } // optional

        [Required]
        [Column(TypeName = "varchar(200)")]
        public required string LocationName { get; set; } // optional => defaults to ((FirstName + " " LastName) || CompanyName) + " " Street | validation less than 200 chars

        [Column(TypeName = "varchar(100)")]
        public string? RecipientName { get; set; } // optional -- but defaults to the Users FullName if not applied 

        [Column(TypeName = "varchar(1000)")]
        public string? DeliveryInstructions { get; set; } // optional

        public required AddressType Type { get; set; } // required upon creation
        public AddressExecutionStatus AddressStatus { get; set; } // defaults to AddressExecutionStatus.NotValidated upon creation 
    }
}
