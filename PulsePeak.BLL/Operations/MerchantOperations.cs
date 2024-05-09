using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.Utils.Extensions;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.AuthModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class MerchantOperations : IMerchantOperations
    {
        private readonly ILogger log;
        private readonly IMapper mapper;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IUserOperations userOperations; // [ALEX] not sure on this; probabaly can be something like IAuthOperatios or so; just a tweak rn
        private string errorMessage;

        public MerchantOperations(ILogger logger, IMapper mapper, IRepositoryHandler repositoryHandler, IUserOperations userOperations)
        {
            this.log = logger;
            this.mapper = mapper;
            this.repositoryHandler = repositoryHandler;
            this.userOperations = userOperations;
            this.errorMessage = string.Empty;
        }

        public async Task<MerchantRegistrationResponseModel> MerchantRegistration(MerchantRegistrationRequestModel merchantRegistrationRequest)
        {
            try {
                // TODO: Move the validation to the API layer
                if (!await IsValidMerchantModel(merchantRegistrationRequest.Merchant)) {
                    throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
                }

                var merchant = this.mapper.Map<MerchantEntity>(merchantRegistrationRequest.Merchant);
                merchant.Active = true;
                merchant.ExecutionStatus = UserExecutionStatus.ACTIVE;
                merchant.IsActive = true;

                var addedMerchant = this.repositoryHandler.MerchantRepository.Add(merchant);

                // TODO [Alex]: Get the token -- this is something for you
                var token = await this.userOperations.Authentication(new AuthenticationRequestModel {
                    UserName = merchantRegistrationRequest.Merchant.UserName,
                    Password = merchantRegistrationRequest.Merchant.Password
                });

                return new MerchantRegistrationResponseModel {
                    Merchant = this.mapper.Map<MerchantModel>(addedMerchant),
                    Token = token.Token,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw new RegistrationException(errorMessage, ex);
            }
        }

        // TODO [ED]: Implement
        public Task<MerchantModel> EditMerchantInfo(MerchantModel merchantModel)
        {
            throw new NotImplementedException();
        }

        public async Task<MerchantModel> GetMerchant(long merchantId)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                        ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                return this.mapper.Map<MerchantModel>(merchant);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<MerchantModel> GetMerchant(string username)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Merchant with username '{username}' not found.");

                return this.mapper.Map<MerchantModel>(merchant);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<AddressModel> GetMerchantBillingAddress(int merchantId)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                    ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                var billingAddress = merchant.BillingAddress;

                return this.mapper.Map<AddressModel>(billingAddress);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<UserExecutionStatus> GetMerchantExecutionStatus(long merchantId)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                        ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                return merchant.ExecutionStatus;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<UserExecutionStatus> GetMerchantExecutionStatus(string username)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Merchant with username '{username}' not found.");

                return merchant.ExecutionStatus;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> IsActive(long merchantId)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                        ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                return merchant.ExecutionStatus == UserExecutionStatus.ACTIVE;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> IsActive(string username)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Merchant with username '{username}' not found.");

                return merchant.ExecutionStatus == UserExecutionStatus.ACTIVE;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task SetMerchantExecutionStatus(long merchantId, UserExecutionStatus status)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                    ?? throw new EntityNotFoundException($"Merchant  with ID '{merchantId}' not found.");

                merchant.ExecutionStatus = status;

                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(merchant)} has not been updated.");
                }
                await this.repositoryHandler.SaveAsync();
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task SetMerchantExecutionStatus(string username, UserExecutionStatus status)
        {
            try {
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.UserName == username)
                        ?? throw new EntityNotFoundException($"Merchant with username '{username}' not found.");

                merchant.ExecutionStatus = status;

                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(merchant)} has not been updated.");
                }
                await this.repositoryHandler.SaveAsync();
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }


        // TODO [ED]: abstract this out, need to be used in the API model as well 
        private async Task<bool> IsValidMerchantModel(MerchantModel merchantModel)
        {
            if (!merchantModel.UserName.IsValidUsername(out errorMessage)) {
                return false;
            }
            // TODO: Check if there is a user with the same username
            // maybe something like await repositoryHandler.IfAny() ??
            bool userNameExists = await this.repositoryHandler.MerchantRepository.IfAnyAsync(x => x.UserName == merchantModel.UserName);
            if (userNameExists) {
                return false;
            }

            if (!merchantModel.Password.IsValidPassword(out errorMessage)) {
                return false;
            }
            if (!merchantModel.EmailAddress.IsValidEmailAddress(out errorMessage)) {
                return false;
            }
            if (!merchantModel.PhoneNumber.IsValidPhoneNumber(out errorMessage)) {
                return false;
            }
            return true;
        }
    }
}