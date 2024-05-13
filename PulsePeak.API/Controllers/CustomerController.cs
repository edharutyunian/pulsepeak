using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.BLL.ViewModels.CustomerViewModels;

namespace PulsePeak.API.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerOperationHandler operationHandler;
        private const string apiPrefix = @"api/v1/";

        public CustomerController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<ICustomerOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this

        // TODO [Alex]: This is a draft for you, take a look into this
        [HttpPost]
        [Route($"{apiPrefix}customerRegistration")]
        public async Task<CustomerRegistrationResponse> CustomerRegistration(CustomerRegistrationRequest customerRegistrationRequest)
        {
            var response = await this.operationHandler.CustomerOperations.CustomerRegistration(customerRegistrationRequest.RequestModel);

            return new CustomerRegistrationResponse() {
                ResponseModel = response
            };
        }
    }
}
