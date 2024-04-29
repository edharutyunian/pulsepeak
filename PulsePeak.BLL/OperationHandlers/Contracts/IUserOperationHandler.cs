using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface IUserOperationHandler
    {
        IUserOperations UserOperations { get; }
    }
}
