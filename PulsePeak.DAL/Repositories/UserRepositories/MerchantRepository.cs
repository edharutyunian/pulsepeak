using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class MerchantRepository : RepositoryBase<MerchantEntity>, IMerchantRepository
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private string errorMessage;

        public MerchantRepository(PulsePeakDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext)
        {
            this.log = logger;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public IQueryable<MerchantEntity> GetAllMerchants()
        {
            try {
                return this.DbContext.Merchants.Where(x => x.IsActive);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }
    }
}
