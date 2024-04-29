using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderOperationHandler operationHandler;

        public OrderController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IOrderOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
