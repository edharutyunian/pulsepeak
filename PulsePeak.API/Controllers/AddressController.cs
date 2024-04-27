using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressOperationHandler addressOperationHandler;

        public AddressController(IServiceProvider serviceProvider)
        {
            this.addressOperationHandler = serviceProvider.GetRequiredService<IAddressOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
