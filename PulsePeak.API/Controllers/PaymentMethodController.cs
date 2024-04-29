using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class PaymentMethodController : Controller
    {
        private IPaymentMethodOperationHandler operationHandler;

        public PaymentMethodController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IPaymentMethodOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
