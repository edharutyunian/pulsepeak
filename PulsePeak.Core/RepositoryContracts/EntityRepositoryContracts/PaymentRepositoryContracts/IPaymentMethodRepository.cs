using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts
{
    public interface IPaymentMethodRepository : IRepositoryBase<PaymentMehodBaseEntity>
    {
        PaymentMethodModel AddPaymentMethod(long customerId, PaymentMethodModel paymentMethodModel);
        IQueryable<PaymentMehodBaseEntity> GetAllActivePayments();
        IQueryable<PaymentMehodBaseEntity> GetAllActivePayments(long customerId);
    }
}
