using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Payments
{
    public class PaymentMehodBaseEntity : EntityBase, IPaymentMethod
    {
        public long OwnerId { get; set; }
        public IUserAccount Owner { get; set; } // should be tied to a User

        public PaymentMethodType PaymentMethodType { get; set; } // defaults to Cash 

        // TODO: [ED] maybe?
        public PaymentMehodBaseEntity()
        {
            this.PaymentMethodType = PaymentMethodType.CreditCard;
        }
        public PaymentMehodBaseEntity(PaymentMethodType paymentMethod) { }
    }
}
