using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class AddressRepository : RepositoryBase<AddressBaseEntity>, IAddressRepository
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private string errorMessage;

        public AddressRepository(PulsePeakDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext)
        {
            this.log = logger;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public AddressModel AddAddress(long userId, AddressModel address)
        {
            try {
                var newAddressEntity = mapper.Map<AddressBaseEntity>(address);
                newAddressEntity.UserId = userId;

                this.DbContext.Addresses.Add(newAddressEntity);

                return mapper.Map<AddressModel>(newAddressEntity);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }
    }
}
