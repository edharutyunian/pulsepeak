using PulsePeak.Core.BLLContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface IUserOperationHandler
    {
        IUserOperations UserOperations { get; }
    }
}
