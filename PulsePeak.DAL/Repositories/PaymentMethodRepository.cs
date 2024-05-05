using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class PaymentMethodRepository : RepositoryBase<PaymentMehodBaseEntity>, IPaymentMethodRepository
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private string errorMessage;

        public PaymentMethodRepository(PulsePeakDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext)
        {
            this.log = logger;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public PaymentMethodModel AddPaymentMethod(long customerId, PaymentMethodModel paymentMethodModel)
        {
            try {
                var paymentEntity = this.mapper.Map<PaymentMehodBaseEntity>(paymentMethodModel);
                paymentEntity.CustomerId = customerId;

                this.DbContext.PaymentMehods.Add(paymentEntity);

                return this.mapper.Map<PaymentMethodModel>(paymentEntity);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while adding an entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
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
