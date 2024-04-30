using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class PaymentMethodRepository : RepositoryBase<PaymentMehodBaseEntity>, IPaymentMethodRepository
    {
        private readonly ILogger log;
        private string errorMessage;

        public PaymentMethodRepository(PulsePeakDbContext dbContext, ILogger logger) : base(dbContext)
        {
            this.errorMessage = string.Empty;
            this.log = logger;
        }

        public IQueryable<PaymentMehodBaseEntity> GetAllActivePayments()
        {
            try {
                return this.DbContext.PaymentMehods.Where(x => x.Active == true);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public IQueryable<PaymentMehodBaseEntity> GetAllActivePayments(long customerId)
        {
            try {
                return this.DbContext.PaymentMehods.Where(x => x.Active == true && x.CustomerId == customerId);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities with the given predicate: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }
    }
}
