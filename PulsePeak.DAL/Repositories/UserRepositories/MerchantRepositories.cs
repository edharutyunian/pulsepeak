using PulsePeak.Core.Entities.Users;
using PulsePeak.DAL.RepositoryContracts;
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
