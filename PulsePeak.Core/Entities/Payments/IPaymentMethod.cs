namespace PulsePeak.Core.Entities.Payments
{
    public interface IPaymentMethod
    {
        long Id { get; set; }
        bool IsPrimary { get; }
        bool IsActive { get; }
    }
}