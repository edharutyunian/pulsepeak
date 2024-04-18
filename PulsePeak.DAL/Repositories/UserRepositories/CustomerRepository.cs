using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class CustomerRepository : RepositoryBase<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(PulsePeakDbContext dbContext) : base(dbContext) { }
    }
}
