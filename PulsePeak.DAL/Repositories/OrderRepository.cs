using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;
using AutoMapper;

namespace PulsePeak.DAL.Repositories
{
    public class OrderRepository : RepositoryBase<OrderBaseEntity>, IOrderRepository
    {
        private readonly IMapper _mapper;

        public OrderRepository(PulsePeakDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
    }
}
