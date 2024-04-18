using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Payments;

namespace PulsePeak.Core.Entities.Users
{
    public class CustomerEntity : EntityBase, IUserAccount
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UsertId { get; set; }
        public required UserBaseEnttity User { get; set; }

        [ForeignKey("Address.Id")]
        public long AddressId { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BirthDate { get; set; }
        public ICollection<IAddress>? ShippingAddresses { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IPaymentMethod>? PaymentMethods { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IOrder>? Orders { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}