using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Utils.Extensions;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class PaymentMethodOperations : IPaymentMethodOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public PaymentMethodOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public async Task<PaymentMethodModel> AddPaymentMethod(long customerId, PaymentMethodModel paymentMethodModel)
        {
            // TODO: Validate model here and move to the API layer as well
            if (!IsValidPaymentMethodModel(paymentMethodModel)) {
                throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
            }

            try {

                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                var payment = this.mapper.Map<PaymentMehodBaseEntity>(paymentMethodModel);
                var addedPayment = this.repositoryHandler.PaymentMethodRepository.Add(payment);

                addedPayment.Customer = customer;

                if (!customer.PaymentMethods.Any(x => x.IsPrimary)) {
                    addedPayment.IsPrimary = true;
                }
                customer.PaymentMethods.Add(addedPayment);

                var isCustoemerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustoemerUpdated) {
                    throw new DbContextException($"The {nameof(CustomerEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<PaymentMethodModel>(addedPayment);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<PaymentMethodModel> EditPaymentMethod(PaymentMethodModel paymentMethodModel)
        {
            // TODO: Validate model here and move to the API layer as well
            try {
                if (!IsValidPaymentMethodModel(paymentMethodModel)) {
                    throw new RegistrationException(errorMessage, new RegistrationException(errorMessage));
                }

                // get the payment method
                var payment = await this.repositoryHandler.PaymentMethodRepository.GetSingleAsync(x => x.Id == paymentMethodModel.Id)
                    ?? throw new EntityNotFoundException($"Payment Method with ID '{paymentMethodModel.Id}' not found.");

                // get the customer
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == paymentMethodModel.CustomerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{paymentMethodModel.CustomerId}' not found.");

                // check if the customer contains that payment method
                var customersPaymentMethod = customer.PaymentMethods.TakeWhile(x => x.Id == payment.Id)
                    ?? throw new KeyNotFoundException($"Customer with ID '{paymentMethodModel.CustomerId}' do not include a payment method with the ID '{paymentMethodModel.Id}'.");

                // map the payment method and update in the DB 
                var editedPayment = this.mapper.Map<PaymentMehodBaseEntity>(paymentMethodModel);
                editedPayment.Customer = customer;

                var isPaymentUpdated = this.repositoryHandler.PaymentMethodRepository.Update(editedPayment);
                if (!isPaymentUpdated) {
                    throw new DbContextException($"The {nameof(PaymentMehodBaseEntity)} has not been updated.");
                }

                // not the best solution I guess, but remove and add the edited payment method; update in the DB
                customer.PaymentMethods.Remove(customersPaymentMethod.First());
                customer.PaymentMethods.Add(editedPayment);

                var isCustomerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustomerUpdated) {
                    throw new DbContextException($"The {nameof(CustomerEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();
                return this.mapper.Map<PaymentMethodModel>(editedPayment);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<PaymentMethodModel> GetPaymentMethod(long paymentMethodId)
        {
            try {
                var payment = await this.repositoryHandler.PaymentMethodRepository.GetSingleAsync(x => x.Id == paymentMethodId)
                    ?? throw new EntityNotFoundException($"Payment with ID '{paymentMethodId}' not found.");

                return this.mapper.Map<PaymentMethodModel>(payment);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ICollection<PaymentMethodModel>> GetCustomerPaymentMethods(long customerId)
        {
            try {
                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == customerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{customerId}' not found.");

                if (customer.PaymentMethods.Count == 0) {
                    throw new KeyNotFoundException($"Customer with ID '{customerId}' do not have any Payment Method added");
                }

                var customersPaymentMetods = new List<PaymentMethodModel>();
                var allPayments = this.repositoryHandler.PaymentMethodRepository.GetAllActivePayments(customerId);

                foreach (var paymentMethod in allPayments) {
                    customersPaymentMetods.Add(this.mapper.Map<PaymentMethodModel>(paymentMethod));
                }

                return customersPaymentMetods;

            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> DeactivatePaymentMethod(long paymentMethodId)
        {
            try {
                var payment = await this.repositoryHandler.PaymentMethodRepository.GetSingleAsync(x => x.Id == paymentMethodId)
                    ?? throw new EntityNotFoundException($"Payment with ID '{paymentMethodId}' not found.");

                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == payment.CustomerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{payment.CustomerId}' not found.");

                payment.Active = false;
                customer.PaymentMethods.Remove(payment);

                var isPaymentUpdated = this.repositoryHandler.PaymentMethodRepository.Update(payment);
                if (!isPaymentUpdated) {
                    throw new DbContextException($"The {nameof(PaymentMehodBaseEntity)} has not been updated.");
                }

                var isCustoemerUpdated = this.repositoryHandler.CustomerRepository.Update(customer);
                if (!isCustoemerUpdated) {
                    throw new DbContextException($"The {nameof(CustomerEntity)} has not been updated.");

                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetPrimaryPaymentMethod(long paymentMethodId)
        {
            try {
                var payment = await this.repositoryHandler.PaymentMethodRepository.GetSingleAsync(x => x.Id == paymentMethodId)
                    ?? throw new EntityNotFoundException($"Payment with ID '{paymentMethodId}' not found.");

                payment.IsPrimary = true;

                var isPaymentUpdated = this.repositoryHandler.PaymentMethodRepository.Update(payment);
                if (!isPaymentUpdated) {
                    throw new DbContextException($"The {nameof(PaymentMehodBaseEntity)} has not been updated.");
                }
                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        // TODO: Implement... Abstract this out, need to be used in the API model as well 
        private bool IsValidPaymentMethodModel(PaymentMethodModel model)
        {
            if (!model.CardNumber.IsValidCreditCardNumber(out errorMessage)) {
                return false;
            }
            if (!model.CardholderName.IsValidName(out errorMessage)) {
                return false;
            }
            // cvv
            // expMonth
            // expYear
            // customerId????
            return true;
        }
    }
}
