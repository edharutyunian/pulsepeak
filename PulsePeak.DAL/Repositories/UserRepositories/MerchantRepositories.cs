using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class MerchantRepositories : RepositoryBase<MerchantEntity>, IMerchantRepository
    {
        public MerchantRepositories(PulsePeakDbContext dbContext) : base(dbContext)
        {
        }
    }
}
