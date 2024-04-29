using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class OrderOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IOrderOperations orderOperations;

        public OrderOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IOrderOperations OrderOperations => orderOperations ?? serviceProvider.GetRequiredService<IOrderOperations>();
    }
}
