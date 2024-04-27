using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class MerchantRepository : RepositoryBase<MerchantEntity>, IMerchantRepository
    {
        public MerchantRepository(PulsePeakDbContext dbContext) : base(dbContext)
        {
        }
    }
}
