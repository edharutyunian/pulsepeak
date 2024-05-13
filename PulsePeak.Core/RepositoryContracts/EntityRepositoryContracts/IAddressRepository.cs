using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts
{
    public interface IAddressRepository : IRepositoryBase<AddressBaseEntity>
    {
        AddressModel AddCustomerAddress(long customerId, AddressModel addressModel);
        AddressModel AddMerchantAddress(long merchantId, AddressModel addressModel);
    }
}
