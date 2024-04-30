using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // TODO: Add and implement any new required methods
    public interface IPaymentMethodOperations
    {
        Task<PaymentMethodModel> AddPaymentMethod(long customerId, PaymentMethodModel paymentMethodModel);
        Task<PaymentMethodModel> EditPaymentMethod(PaymentMethodModel paymentMethodModel);
        Task<bool> DeactivatePaymentMethod(long paymentMethodId);
        Task<PaymentMethodModel> GetPaymentMethod(long paymentMethodId);
        Task<ICollection<PaymentMethodModel>> GetCustomerPaymentMethods(long customerId);
        Task<bool> SetPrimaryPaymentMethod(long paymentMethodId);
    }
}