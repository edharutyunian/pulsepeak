using PulsePeak.Core.Entities.Users;
using PulsePeak.DAL.RepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories.UserRepositories
{
    public class CustomerRepository : RepositoryBase<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(PulsePeakDbContext dbContext) : base(dbContext) { }
    }
}
