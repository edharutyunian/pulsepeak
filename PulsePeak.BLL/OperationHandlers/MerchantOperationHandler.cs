using Microsoft.Extensions.DependencyInjection;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class MerchantOperationHandler : IMerchantOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMerchantOperations merchantOperations;

        public MerchantOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMerchantOperations MerchantOperations => this.merchantOperations ?? this.serviceProvider.GetRequiredService<IMerchantOperations>();
    }
}
