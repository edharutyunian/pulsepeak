using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.BLL.ViewModels.CustomerViewModels;
using PulsePeak.BLL.ViewModels.MerchantViewModels;

namespace PulsePeak.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserOperationHandler operationHandler;
        private const string apiPrefix = @"api/v1/";

        public UserController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IUserOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this

        // TODO [Alex]: This is a draft for you, take a look into this
        [HttpPost]
        [Route($"{apiPrefix}customerRegistration")]
        public async Task<CustomerRegistrationResponse> CustomerRegistration(CustomerRegistrationRequest customerRegistrationRequest)
        {
            var response = await this.operationHandler.UserOperations.CustomerRegistration(customerRegistrationRequest.RequestModel);

            return new CustomerRegistrationResponse() {
                ResponseModel = response
            };
        }

        // TODO [Alex]: This is a draft for you, take a look into this
        [HttpPost]
        [Route($"{apiPrefix}merchantRegistration")]
        public async Task<MerchantRegistrationResponse> MerchantRegistration(MerchantRegistrationRequest merchantRegistrationRequest)
        {
            var response = await this.operationHandler.UserOperations.MerchantRegistration(merchantRegistrationRequest.RequestModel);

            return new MerchantRegistrationResponse() {
                ResponseModel = response
            };
        }
    }
}
