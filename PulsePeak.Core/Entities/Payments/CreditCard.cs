using System.ComponentModel.DataAnnotations;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Payments
{
    public class CreditCard : EntityBase, IPaymentMethod
    {
        public long OwnerId { get; set; }
        public IUserAccount Owner { get; set; }

        public long PaymentMehodId { get; set; }
        public PaymentMehodBaseEntity PaymentMehod { get; set; }

        [Required]
        public string CardNumber { get; set; } // required -- validation can be found in the BLL.Utils or similar 
        [Required]
        public string HolderName { get; set; } // required 
        [Required]
        public string CardName { get; set; } // optional thing. not sure about this 
        [Required]
        public byte ExpirationMonth { get; set; } // validation [1>x<12]
        [Required]
        public byte ExpirationYear { get; set; } // validation -- should not be less than current date, and more than 5 years
        [Required]
        public short CVV { get; set; } // validate that this is a 3digit number
    }
}
