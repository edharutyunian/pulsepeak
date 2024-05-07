using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts
{
    public interface IOrderRepository : IRepositoryBase<OrderBaseEntity>
    {
        OrderModel AddOrder(long customerId, ShoppingCartModel shoppingCartModel);
    }
}
