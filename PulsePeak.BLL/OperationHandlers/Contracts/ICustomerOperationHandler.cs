using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface ICustomerOperationHandler
    {
        ICustomerOperations CustomerOperations { get; }
    }
}
