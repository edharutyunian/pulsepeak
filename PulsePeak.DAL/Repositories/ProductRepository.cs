using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.DAL.RepositoryImplementation;
using AutoMapper;

namespace PulsePeak.DAL.Repositories
{
    public class ProductRepository : RepositoryBase<ProductBaseEntity>, IProductRepository
    {
        private readonly IMapper _mapper;

        public ProductRepository(PulsePeakDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
    }
}
