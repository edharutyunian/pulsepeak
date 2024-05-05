using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    [Table("Merchants")]
    public class MerchantEntity : EntityBase, IUserAccount
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UserId { get; set; }
        public required UserBaseEnttity User { get; set; }

        [Required]
        [ForeignKey("BillingAddress.Id")]
        public long BillingAddressId { get; set; }
        public IAddress BillingAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string CompanyName { get; set; }

        public string? FirstName { get; set; } // this is basically the POC or owner first name
        public string? LastName { get; set; } // this is basically the POC or owner last name
        public string POCName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }

        [Required]
        [Column(TypeName = "varchar(130)")]
        public required string EmailAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public required string PhoneNumber { get; set; }

        [Required]
        public OrganizationType OrganizationType { get; set; } // required
        public ICollection<ProductBaseEntity> Store { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}
