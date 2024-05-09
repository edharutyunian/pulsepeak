using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.BLL.ViewModels.MerchantViewModels;

namespace PulsePeak.API.Controllers
{
    public class MerchantController : Controller
    {
        private readonly IMerchantOperationHandler operationHandler;
        private const string apiPrefix = @"api/v1/";

        public MerchantController(IServiceProvider serviceProvider)
        {
            this.operationHandler = serviceProvider.GetRequiredService<IMerchantOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this

        // TODO [Alex]: This is a draft for you, take a look into this
        [HttpPost]
        [Route($"{apiPrefix}merchantRegistration")]
        public async Task<MerchantRegistrationResponse> MerchantRegistration(MerchantRegistrationRequest merchantRegistrationRequest)
        {
            var response = await this.operationHandler.MerchantOperations.MerchantRegistration(merchantRegistrationRequest.RequestModel);

            return new MerchantRegistrationResponse() {
                ResponseModel = response
            };
        }
    }
}
