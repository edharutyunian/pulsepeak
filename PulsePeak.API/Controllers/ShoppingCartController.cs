using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartOperationHandler shoppingCartOperationHandler;

        public ShoppingCartController(IServiceProvider serviceProvider)
        {
            this.shoppingCartOperationHandler = serviceProvider.GetRequiredService<IShoppingCartOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
