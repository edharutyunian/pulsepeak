using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Payments;

namespace PulsePeak.Core.Entities.Users
{
    [Table("Customers")]
    public class CustomerEntity : EntityBase, IUserAccount
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
        [ForeignKey("ShippingAddress.Id")]
        public long ShippingAddressId { get; set; }
        public IAddress ShippingAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string LastName { get; set; }

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
        public ICollection<IPaymentMethod> PaymentMethods { get; set; } // can be disregarded upon User creation | required only for checking out 
        public IPaymentMethod PrimaryPaymentMethod => PaymentMethods.FirstOrDefault(x => x.IsPrimary == true) ?? PaymentMethods.FirstOrDefault(x => x.IsActive == true);
        public ICollection<IOrder>? Orders { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}