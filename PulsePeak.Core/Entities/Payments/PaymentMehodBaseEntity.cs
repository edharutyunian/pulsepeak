using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Payments
{
    [Table("PaymentMethods")]
    public class PaymentMehodBaseEntity : EntityBase, IPaymentMethod
    {
        [Required]
        [ForeignKey("Owner.Id")]
        public long UserId { get; set; }
        public required IUserAccount Owner { get; set; }

        public PaymentMethodType PaymentMethodType { get; set; } // defaults to Cash 

        // TODO: [ED] maybe?
        public PaymentMehodBaseEntity()
        {
            this.PaymentMethodType = PaymentMethodType.CreditCard;
        }
        public PaymentMehodBaseEntity(PaymentMethodType paymentMethod)
        {
            this.PaymentMethodType = paymentMethod;
        }
    }
}
