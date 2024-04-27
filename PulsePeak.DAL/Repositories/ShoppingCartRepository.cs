using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;
using AutoMapper;

namespace PulsePeak.DAL.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartBaseEntity>, IShoppingCartRepository
    {
        private readonly IMapper _mapper;

        public ShoppingCartRepository(PulsePeakDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
    }
}
