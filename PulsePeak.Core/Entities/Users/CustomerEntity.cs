using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Addresses;

namespace PulsePeak.Core.Entities.Users
{
    [Table("Customers")]
    public class CustomerEntity : EntityBase, IUserAccount
    {
        //[Required]
        //[ForeignKey("User.Id")]
        //public long UserId { get; set; }
        //public required UserBaseEnttity User { get; set; }

        [Required]
        [ForeignKey("BillingAddress.Id")]
        public long BillingAddressId { get; set; }
        public AddressBaseEntity BillingAddress { get; set; }

        [Required]
        [ForeignKey("ShippingAddress.Id")]
        public long ShippingAddressId { get; set; }
        public AddressBaseEntity ShippingAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string LastName { get; set; }

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

        [Column(TypeName = "datetime")]
        public DateTime BirthDate { get; set; }

        // TODO [ED]: update to IDictionarry<string, IPaymentMethod>
        // I guess it would be better to use IDictionarry<string, IPaymentMethod> with 'string' as a cardNumber to avoid any possible duplicate cards
        public ICollection<PaymentMehodBaseEntity?> PaymentMethods { get; set; }
        public PaymentMehodBaseEntity? PrimaryPaymentMethod => PaymentMethods.FirstOrDefault(x => x.IsActive);
        public ICollection<OrderBaseEntity>? Orders { get; set; }

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