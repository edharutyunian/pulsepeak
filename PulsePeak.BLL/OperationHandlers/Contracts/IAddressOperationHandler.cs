using PulsePeak.Core.BLLOperationContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface IAddressOperationHandler
    {
        IAddressOperations AddressOperations { get; }
    }
}
