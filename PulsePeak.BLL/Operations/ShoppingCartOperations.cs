using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class ShoppingCartOperations : IShoppingCartOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public ShoppingCartOperations(ILogger log, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = log;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }

        public async Task<ShoppingCartModel> AddProductToCart(ShoppingCartModel shoppingCartModel, long productId, int quantity)
        {
            try {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // check the shopping Cart
                var shoppingCartEntity = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartModel.Id)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartModel.Id}' not found.");

                // check the product
                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                var productModel = this.mapper.Map<ProductModel>(product);

                productModel.AddedQuantity += quantity;
                shoppingCartModel.Products.Add(productModel);
                shoppingCartModel.TotalItemCount++;

                var shoppingCart = this.mapper.Map<ShoppingCartBaseEntity>(shoppingCartModel);

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(product)} has not been updated.");
                }

                var isShoppingCartUpdated = this.repositoryHandler.ShoppingCartRepository.Update(shoppingCart);
                if (!isShoppingCartUpdated) {
                    throw new DbContextException($"The {nameof(shoppingCart)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return shoppingCartModel;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ShoppingCartModel> RemoveProductFromCart(ShoppingCartModel shoppingCartModel, long productId)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // check if the shoppingCart contains the product
                if (!shoppingCartModel.Products.Any(x => x.Id == productId)) {
                    throw new KeyNotFoundException($"Shopping Cart with ID '{shoppingCartModel.CustomerId}' do not contain a product with the ID '{productId}'.");
                }

                // check the shopping Cart
                var shoppingCartEntity = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartModel.Id)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartModel.Id}' not found.");

                // check the product
                var productEntity = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                var productModel = this.mapper.Map<ProductModel>(productEntity);

                productModel.AddedQuantity = 0;
                shoppingCartModel.Products.Remove(productModel);
                shoppingCartModel.TotalItemCount--;

                var shoppingCart = this.mapper.Map<ShoppingCartBaseEntity>(shoppingCartModel);

                var isShoppingCartUpdated = this.repositoryHandler.ShoppingCartRepository.Update(shoppingCart);
                if (!isShoppingCartUpdated) {
                    throw new DbContextException($"The {nameof(shoppingCart)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<ShoppingCartModel>(shoppingCart);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ShoppingCartModel> ClearCart(ShoppingCartModel shoppingCartModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                // check the shopping Cart
                var shoppingCartEntity = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartModel.Id)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartModel.Id}' not found.");

                // TODO: Is there a need to default other properties as well? || maybe it's better to just not show all others in the UI, like address, payment and so on
                // clear shopping cart
                shoppingCartModel.Products.Clear();
                shoppingCartModel.TotalItemCount = 0;

                var shoppingCart = this.mapper.Map<ShoppingCartBaseEntity>(shoppingCartModel);

                var isShoppingCartUpdated = this.repositoryHandler.ShoppingCartRepository.Update(shoppingCart);
                if (!isShoppingCartUpdated) {
                    throw new DbContextException($"The {nameof(shoppingCart)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return this.mapper.Map<ShoppingCartModel>(shoppingCart);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ShoppingCartModel> GetCart(long shoppingCartId)
        {
            try {
                var shoppingCart = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartId)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartId}' not found.");

                return this.mapper.Map<ShoppingCartModel>(shoppingCart);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public decimal GetTotalPrice(ShoppingCartModel shoppingCartModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                return shoppingCartModel.TotalPrice;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<decimal> GetTotalPrice(long shoppingCartId)
        {
            try {
                var shoppingCart = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartId)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartId}' not found.");

                var shoppingCartModel = this.mapper.Map<ShoppingCartModel>(shoppingCart);

                return shoppingCartModel.TotalPrice;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ShoppingCartModel> SetPaymentMethod(ShoppingCartModel shoppingCartModel, long paymentMethodId)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                var shoppingCart = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartModel.Id)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartModel.Id}' not found.");

                var paymentMethod = await this.repositoryHandler.PaymentMethodRepository.GetSingleAsync(x => x.Id == paymentMethodId)
                    ?? throw new EntityNotFoundException($"Payment Method with ID '{paymentMethodId}' not found.");

                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == shoppingCartModel.CustomerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{shoppingCartModel.CustomerId}' not found.");

                // check if the customer contains that payment
                if (!customer.PaymentMethods.Contains(paymentMethod)) {
                    throw new KeyNotFoundException($"Customer with ID '{customer.Id}' do not contain a payment method with the ID '{paymentMethodId}'.");
                }

                var paymentMethodModel = this.mapper.Map<PaymentMethodModel>(paymentMethod);

                // update
                shoppingCartModel.PaymentMethod = paymentMethodModel;

                var shoppingCartEntity = this.mapper.Map<ShoppingCartBaseEntity>(shoppingCartModel);

                var isShoppingCartUpdated = this.repositoryHandler.ShoppingCartRepository.Update(shoppingCartEntity);
                if (!isShoppingCartUpdated) {
                    throw new DbContextException($"The {nameof(shoppingCartEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return shoppingCartModel;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ShoppingCartModel> SetShippingAddress(ShoppingCartModel shoppingCartModel, long shippingAddressId)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidShoppingCartModel(shoppingCartModel)) {
                    throw new EditableArgumentException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                var shoppingCart = await this.repositoryHandler.ShoppingCartRepository.GetSingleAsync(x => x.Id == shoppingCartModel.Id)
                    ?? throw new EntityNotFoundException($"ShoppingCart with ID '{shoppingCartModel.Id}' not found.");

                var customer = await this.repositoryHandler.CustomerRepository.GetSingleAsync(x => x.Id == shoppingCartModel.CustomerId)
                    ?? throw new EntityNotFoundException($"Customer with ID '{shoppingCartModel.CustomerId}' not found.");

                // check if the customer contains that payment
                if (customer.ShippingAddressId != shippingAddressId) {
                    throw new KeyNotFoundException($"Customer with ID '{customer.Id}' do not contain a shipping address with the ID '{shippingAddressId}'.");
                }

                var shippingAddress = await this.repositoryHandler.AddressRepository.GetSingleAsync(x => x.Id == shippingAddressId)
                    ?? throw new EntityNotFoundException($"Address with ID '{shippingAddressId}' not found.");

                var shippingAddressModel = this.mapper.Map<AddressModel>(shippingAddress);

                // update
                shoppingCartModel.ShippingAddress = shippingAddressModel;

                var shoppingCartEntity = this.mapper.Map<ShoppingCartBaseEntity>(shoppingCartModel);

                var isShoppingCartUpdated = this.repositoryHandler.ShoppingCartRepository.Update(shoppingCartEntity);
                if (!isShoppingCartUpdated) {
                    throw new DbContextException($"The {nameof(shoppingCartEntity)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return shoppingCartModel;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
            throw new NotImplementedException();
        }

        // TODO: Implement... Abstract this out, need to be used in the API model as well 
        private bool IsValidShoppingCartModel(ShoppingCartModel model)
        {
            return false;
        }
    }
}
