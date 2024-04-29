using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressOperationHandler operationHandler;

        public AddressController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IAddressOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
