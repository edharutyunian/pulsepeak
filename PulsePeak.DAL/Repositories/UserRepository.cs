using PulsePeak.Core.Entities.Users;
using PulsePeak.DAL.RepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class UserRepository : RepositoryBase<UserBaseEnttity>, IUserRepository
    {
        public UserRepository(PulsePeakDbContext dbContext) : base(dbContext)
        {
        }
    }
}
