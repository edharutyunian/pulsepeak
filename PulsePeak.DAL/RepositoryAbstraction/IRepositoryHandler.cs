using PulsePeak.DAL.RepositoryContracts;

namespace PulsePeak.DAL.RepositoryAbstraction
{
    public interface IRepositoryHandler
    {
        // Arsen -- something for you to take care of 

        // here should be all the IRepo** from RepositoryContracts like IUserReporsitory User {get; } and so on

        IUserRepository UserRepository { get; }

    }
}
