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
        [ForeignKey("BillingAddress.Id")]
        public long BillingAddressId { get; set; }
        public AddressBaseEntity BillingAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string CompanyName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string? FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string? LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        [Required]
        [Column("Username", TypeName = "varchar(20)")]
        public required string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public required string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(130)")]
        public required string EmailAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public required string PhoneNumber { get; set; }

        [Required]
        public OrganizationType OrganizationType { get; set; } // required
        public ICollection<ProductBaseEntity> Store { get; set; } // can be disregarded upon User creation | required only for checking out 

        public UserExecutionStatus ExecutionStatus { get; set; }
        public bool IsActive {
            get {
                return this.ExecutionStatus == UserExecutionStatus.ACTIVE;
            }
            set { }
        }
        public bool IsBlocked {
            get {
                return this.ExecutionStatus == UserExecutionStatus.BLOCKED;
            }
            set { }
        }
        public bool IsDeleted {
            get {
                return this.ExecutionStatus == UserExecutionStatus.DELETED;
            }
            set { }
        }
    }
}