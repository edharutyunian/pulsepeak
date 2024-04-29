using AutoMapper;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class PaymentMethodOperations : IPaymentMethodOperations
    {
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public PaymentMethodOperations(IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }


    }
}
