using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    // TODO: Add and implement any new required methods
    public interface IOrderOperations
    {
        Task<OrderModel> CreateOrder(ShoppingCartModel shoppingCart);
        Task<OrderModel> Checkout(long orderId, ShoppingCartModel shoppingCartModel);
        Task<OrderModel> GetOrder(long orderId);
        Task<IEnumerable<OrderModel>> GetCustomerOrders(long customerId);
        Task<bool> MarkAsShipped(long orderId);
        Task<bool> MarkAsDelivered(long orderId);
        Task<bool> MarkAsCanceled(long orderId);
        Task<bool> UpdateOrderStatus(long orderId, OrderPlacementStatus orderPlacementStatus);
    }
}