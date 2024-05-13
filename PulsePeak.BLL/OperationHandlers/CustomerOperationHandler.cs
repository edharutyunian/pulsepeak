using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.BLL.OperationHandlers.Contracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class CustomerOperationHandler : ICustomerOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICustomerOperations customerOperations;

        public CustomerOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICustomerOperations CustomerOperations => this.customerOperations ?? this.serviceProvider.GetRequiredService<ICustomerOperations>();
    }
}
