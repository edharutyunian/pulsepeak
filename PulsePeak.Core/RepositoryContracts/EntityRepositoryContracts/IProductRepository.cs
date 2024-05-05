using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts
{
    public interface IProductRepository : IRepositoryBase<ProductBaseEntity>
    {
        ProductModel AddProduct(long merchantId, ProductModel productModel);
    }
}
