using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class AddressRepository : RepositoryBase<AddressBaseEntity>, IAddressRepository
    {
        public AddressRepository(PulsePeakDbContext dbContext) : base(dbContext)
        {
        }

        public AddressModel AddAddress(long userId, AddressModel address)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
