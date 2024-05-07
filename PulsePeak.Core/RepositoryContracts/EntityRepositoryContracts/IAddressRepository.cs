using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts
{
    public interface IAddressRepository : IRepositoryBase<AddressBaseEntity>
    {
        AddressModel AddAddress(long userId, AddressModel address);
    }
}
