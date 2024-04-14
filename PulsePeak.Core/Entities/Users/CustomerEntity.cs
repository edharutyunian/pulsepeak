using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Payments;

namespace PulsePeak.Core.Entities.Users
{
    public class CustomerEntity : EntityBase, IUserAccount
    {
        public long UsertId { get; set; }
        public UserBaseEnttity User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IAddress>? ShippingAddresses { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IPaymentMethod>? PaymentMethods { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IOrder>? Orders { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}