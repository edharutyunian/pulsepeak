using Microsoft.Extensions.DependencyInjection;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class UserOperationHandler : IUserOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserOperations userOperations;

        public UserOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IUserOperations UserOperations => this.userOperations ?? this.serviceProvider.GetRequiredService<IUserOperations>();
    }
}
