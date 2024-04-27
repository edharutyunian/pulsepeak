using Microsoft.AspNetCore.Mvc;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserOperationHandler userOperations;

        public UserController(IServiceProvider serviceProvider)
        {
            this.userOperations = serviceProvider.GetRequiredService<IUserOperationHandler>();
        }

        // TODO [Alex]: I guess the controller need to be something like this
    }
}
