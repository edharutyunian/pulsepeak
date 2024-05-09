using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface IMerchantOperationHandler
    {
        IMerchantOperations MerchantOperations { get; }
    }
}