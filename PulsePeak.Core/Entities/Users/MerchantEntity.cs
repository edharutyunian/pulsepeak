using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    public class MerchantEntity : EntityBase, IUserAccount
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UserId { get; set; }
        public required UserBaseEnttity User { get; set; }

        public string? FirstName { get; set; } // this is basically the POC or owner first name
        public string? LastName { get; set; } // this is basically the POC or owner last name

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string CompanyName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string POCName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        [Required]
        public OrganizationType OrganizationType { get; set; } // required
        public ICollection<IProduct>? Store { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}
