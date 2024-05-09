using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.ViewModels.AddressModels;
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

        public AddressModel AddCustomerAddress(long customerId, AddressModel addressModel)
        {
            try {
                var address = this.mapper.Map<AddressBaseEntity>(addressModel);

                address.CustomerId = customerId;

                this.DbContext.Addresses.Add(address);

                return this.mapper.Map<AddressModel>(address);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while adding an entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public AddressModel AddMerchantAddress(long merchantId, AddressModel addressModel)
        {
            try {
                var address = this.mapper.Map<AddressBaseEntity>(addressModel);

                address.MerchantId = merchantId;

                this.DbContext.Addresses.Add(address);

                return this.mapper.Map<AddressModel>(address);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while adding an entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }
    }
}
