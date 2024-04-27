using PulsePeak.Core.BLLContracts;

namespace PulsePeak.BLL.OperationHandlers.Contracts
{
    public interface IAddressOperationHandler
    {
        IAddressOperations AddressOperations { get; }
    }
}
