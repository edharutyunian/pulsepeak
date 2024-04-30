using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.ViewModels
{
    public class PaymentMethodModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public required string CardNumber { get; set; }
        public required string CardholderName { get; set; }
        public required byte ExpirationMonth { get; set; }
        public required short ExpirationYear { get; set; }
        public short CVV { get; set; }
        public string? CardName { get; set; }
        public required bool IsPrimary { get; set; }
    }
}
