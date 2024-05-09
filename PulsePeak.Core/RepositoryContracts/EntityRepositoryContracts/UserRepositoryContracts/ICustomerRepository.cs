using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {

        IQueryable<CustomerEntity> GetAllCustomers();
    }
}
