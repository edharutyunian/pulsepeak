using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductOperationHandler operationHandler;

        public ProductController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IProductOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
