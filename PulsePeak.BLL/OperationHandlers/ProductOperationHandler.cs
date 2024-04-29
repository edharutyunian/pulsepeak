using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class ProductOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IProductOperations productOperations;

        public ProductOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IProductOperations ProductOperations => this.productOperations ?? this.serviceProvider.GetRequiredService<IProductOperations>();
    }
}
