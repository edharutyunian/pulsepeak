using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;
using AutoMapper;

namespace PulsePeak.DAL.Repositories
{
    public class AddressRepository : RepositoryBase<AddressBaseEntity>, IAddressRepository
    {
        private readonly IMapper _mapper;

        public AddressRepository(PulsePeakDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public AddressModel AddAddress(long userId, AddressModel address)
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new ArgumentException("User not found", nameof(userId));

            var newAddressEntity = _mapper.Map<AddressBaseEntity>(address);
            newAddressEntity.UserId = userId;

            this.DbContext.Addresses.Add(newAddressEntity);
            //this.DbContext.SaveChanges();

            return _mapper.Map<AddressModel>(newAddressEntity);
        }
    }
}
