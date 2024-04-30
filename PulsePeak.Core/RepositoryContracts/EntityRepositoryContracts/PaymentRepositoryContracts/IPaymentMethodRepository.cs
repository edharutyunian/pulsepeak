using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts
{
    public interface IPaymentMethodRepository : IRepositoryBase<PaymentMehodBaseEntity>
    {
        IQueryable<PaymentMehodBaseEntity> GetAllActivePayments();
        IQueryable<PaymentMehodBaseEntity> GetAllActivePayments(long customerId);
    }
}
