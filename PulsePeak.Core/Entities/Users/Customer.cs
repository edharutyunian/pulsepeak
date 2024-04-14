using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Payments;

namespace PulsePeak.Core.Entities.Users
{
    public class Customer : UserBaseEnttity, IUserAccount
    {
        public DateTime BirthDate { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IAddress>? ShippingAddresses { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IPaymentMethod>? PaymentMethods { get; set; } // can be disregarded upon User creation | required only for checking out 
        public ICollection<IOrder>? Orders { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}