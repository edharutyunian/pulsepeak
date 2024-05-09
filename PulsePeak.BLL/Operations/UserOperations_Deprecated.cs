using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels.AuthModels;

namespace PulsePeak.BLL.Operations
{
    // Deprecated => Just left here for the auth stuff
    public class UserOperations_Deprecated : IUserOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        // need to add something like TokenKey and TokenParameters or so for the Auth
        private string errorMessage;

        public UserOperations_Deprecated(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public Task<AuthResponse> Authentication(AuthenticationRequestModel authenticationRequest)
        {
            // TODO [Alex]: this is something for you to take care of
            throw new NotImplementedException();
        }


        public Task<AuthResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            // TODO [Alex]: take a look into this 
            throw new NotImplementedException();
        }
    }
}