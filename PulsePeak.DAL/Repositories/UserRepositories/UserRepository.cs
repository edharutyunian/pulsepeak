using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class UserRepository : RepositoryBase<UserBaseEnttity>, IUserRepository
    {
        public UserRepository(PulsePeakDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<UserBaseEnttity> GetAllUsers()
        { 
            // something like DbContext.users.where(x => x.ExecutionStatus == Active )
            throw new NotImplementedException();    
        }
    } 
}
