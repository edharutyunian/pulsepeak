using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // TODO: Add and implement any new required methods
    public interface IProductOperations
    {
        Task<ProductModel> AddProduct(long merchantId, ProductModel productModel);
        Task<ProductModel> EditProduct(ProductModel productModel);
        Task DeactivateProduct(long productId);
        Task<ProductModel> GetProduct(long productId);
        Task<bool> SetTotalAvailableQuantity(long productId, int quantity);
        Task<bool> SetMinQuantityPerOrder(long productId, int quantity);
        Task<bool> SetMaxQuantityPerOrder(long productId, int quantity);
        Task<bool> SetPrice(long productId, decimal price);
        Task<bool> SetProductAvailabilityStatus(long productId, ProductAvailabilityStatus productAvailabilityStatus);
    }
}
