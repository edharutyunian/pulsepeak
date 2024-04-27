using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class ShoppingCartOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IShoppingCartOperations shoppingCartOperations;

        public ShoppingCartOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IShoppingCartOperations ShoppingCartOperations => this.shoppingCartOperations ?? this.serviceProvider.GetRequiredService<IShoppingCartOperations>();
    }
}
