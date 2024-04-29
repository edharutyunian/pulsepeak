using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers
{
    public class PaymentMethodOperationHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IPaymentMethodOperations paymentMethodOperations;

        public PaymentMethodOperationHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IPaymentMethodOperations PaymentMethodOperations => this.paymentMethodOperations ?? this.serviceProvider.GetRequiredService<IPaymentMethodOperations>();
    }
}
