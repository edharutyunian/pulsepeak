using Microsoft.Extensions.DependencyInjection;
using PulsePeak.BLL.OperationHandlers.Contracts;
using PulsePeak.Core.BLLContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class AddressOperationHandler : IAddressOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IAddressOperations addressOperations;

        public AddressOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IAddressOperations AddressOperations => this.addressOperations ?? this.serviceProvider.GetRequiredService<IAddressOperations>();
    }
}
