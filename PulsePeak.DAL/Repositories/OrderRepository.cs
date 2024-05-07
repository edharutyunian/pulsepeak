using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;

namespace PulsePeak.DAL.Repositories
{
    public class OrderRepository : RepositoryBase<OrderBaseEntity>, IOrderRepository
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private string errorMessage;

        public OrderRepository(PulsePeakDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext)
        {
            this.log = logger;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        // TODO: Implement
        public OrderModel AddOrder(long customerId, ShoppingCartModel shoppingCartModel)
        {
            throw new NotImplementedException();
        }
    }
}
