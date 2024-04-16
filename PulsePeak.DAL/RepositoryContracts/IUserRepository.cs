using PulsePeak.Core.Entities.Users;
using PulsePeak.DAL.RepositoryAbstraction;

namespace PulsePeak.DAL.RepositoryContracts
{
    public interface IUserRepository : IRepositoryBase<UserBaseEnttity>
    {
        IQueryable<UserBaseEnttity> GetAllUsers();
    }
}
