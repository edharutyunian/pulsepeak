using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Payments
{
    public class PaymentMehodBaseEntity : EntityBase, IPaymentMethod
    {
        public PaymentMethodType PaymentMethodType { get; set; } // defaults to Cash 
        public IUserAccount Owner { get; set; } // should be tied to a User

        // maybe?
        public PaymentMehodBaseEntity()
        {
            this.PaymentMethodType = PaymentMethodType.CreditCard;
        }
        public PaymentMehodBaseEntity(PaymentMethodType paymentMethod) { }
    }
}
