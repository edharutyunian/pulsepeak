namespace PulsePeak.Core.Entities.Payments
{
    public class CreditCard : PaymentMehodBaseEntity, IPaymentMethod
    {
        public string CardNumber { get; set; } // required -- validation can be found in the BLL.Utils or similar 
        public string HolderName { get; set; } // required 
        public string CardName { get; set; } // optional thing. not sure about this 
        public byte ExpirationMonth { get; set; } // validation [1>x<12]
        public byte ExpirationYear { get; set; } // validation -- should not be less than current date, and more than 5 years
        public short CVV { get; set; } // validate that this is a 3digit number
    }
}
