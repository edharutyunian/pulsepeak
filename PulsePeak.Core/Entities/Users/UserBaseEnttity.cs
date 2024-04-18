using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Contacts;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    [Table("Users")]
    public class UserBaseEnttity : EntityBase, IUserAccount
    {
        [Required]
        [ForeignKey("Customer.Id")]
        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }

        [Required]
        [ForeignKey("Merchnt.Id")]
        public long MerchantId { get; set; }
        public MerchantEntity Merchant { get; set; }

        [Required]
        [ForeignKey("BillingAddress.Id")]
        public long BillingAddressId { get; set; }
        public IAddress BillingAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string FirstName { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string LastName { get; set; }
        public string FullName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        [Required]
        [Column("Username", TypeName = "varchar(20)")]
        public required string UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public required string Password { get; set; }
        public ICollection<IContact> Contacts { get; set; }
        [Required]
        public UserType Type { get; set; }
        public UserExecutionStatus ExecutionStatus { get; set; }
        //public byte[] ProfilePicture { get; set; } // let's just not do this for now

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
    }
}