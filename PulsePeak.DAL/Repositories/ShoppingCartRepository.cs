using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartBaseEntity>, IShoppingCartRepository
    {
        private readonly ILogger log;
        private readonly IMapper mapper;

        public ShoppingCartRepository(PulsePeakDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext)
        {
            this.log = logger;
            this.mapper = mapper;
        }
    }
}
