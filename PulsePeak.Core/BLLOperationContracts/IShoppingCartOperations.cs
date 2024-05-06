using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    public interface IShoppingCartOperations
    {
        Task<ShoppingCartModel> AddProductToCart(ShoppingCartModel shoppingCartModel, long productId, int quantity);
        Task<ShoppingCartModel> RemoveProductFromCart(ShoppingCartModel shoppingCartModel, long productId);
        Task<ShoppingCartModel> ClearCart(ShoppingCartModel shoppingCartModel);
        Task<ShoppingCartModel> GetCart(long shoppingCartId);
        Task<ShoppingCartModel> SetShippingAddress(ShoppingCartModel shoppingCartModel, long shippingAddressId);
        Task<ShoppingCartModel> SetPaymentMethod(ShoppingCartModel shoppingCartModel, long paymentMethodId);
        decimal GetTotalPrice(ShoppingCartModel shoppingCartModel);
        Task<decimal> GetTotalPrice(long shoppingCartId);
    }
}