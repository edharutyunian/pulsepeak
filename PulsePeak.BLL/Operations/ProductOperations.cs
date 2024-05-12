using AutoMapper;
using Microsoft.Extensions.Logging;
using PulsePeak.Core.Utils;
using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.BLL.Operations
{
    public class ProductOperations : IProductOperations
    {
        private readonly ILogger log;
        private readonly IRepositoryHandler repositoryHandler;
        private readonly IMapper mapper;
        private string errorMessage;

        public ProductOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }


        public async Task<ProductModel> AddProduct(long merchantId, ProductModel productModel)
        {
            try {
                // TODO: Validate model here and move to the API layer as well
                if (!IsValidProductModel(productModel)) {
                    throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
                }

                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == merchantId)
                    ?? throw new EntityNotFoundException($"Merchant with ID '{merchantId}' not found.");

                if (productModel.MerchantId != merchant.Id) {
                    throw new RegistrationException($"Model's Merchant ID '{productModel.MerchantId}' and provided Merchant ID '{merchant.Id}' are not matching.");
                }

                // add the product
                var addedProduct = this.repositoryHandler.ProductRepository.AddProduct(merchantId, productModel);

                // update merchant entity
                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(merchant)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();

                return addedProduct;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task DeactivateProduct(long productId)
        {
            try {
                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.Active = false;
                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }
                await this.repositoryHandler.SaveAsync();
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ProductModel> EditProduct(ProductModel productModel)
        {
            // TODO: Validate model here and move to the API layer as well
            if (!IsValidProductModel(productModel)) {
                throw new RegistrationException(this.errorMessage, new RegistrationException(this.errorMessage));
            }
            try {
                // get the product
                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productModel.Id)
                    ?? throw new EntityNotFoundException($"Product with ID '{productModel.Id}' not found.");

                // get the merchant
                var merchant = await this.repositoryHandler.MerchantRepository.GetSingleAsync(x => x.Id == productModel.MerchantId)
                    ?? throw new EntityNotFoundException($"Merchant with ID '{productModel.MerchantId}' not found.");

                // check if the merchant contains that product
                var merchantsProduct = merchant.Store.TakeWhile(x => x.Id == product.Id)
                    ?? throw new KeyNotFoundException($"Merchant with ID '{merchant.Id}' do not contain a Product with the ID '{product.Id}'.");

                // map the payment method and update in the DB 
                var editedProduct = this.mapper.Map<ProductBaseEntity>(productModel);
                editedProduct.Merchant = merchant;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(editedProduct);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(editedProduct)} has not been updated.");
                }

                // not the best solution I guess, but remove and add the edited payment method; update in the DB
                merchant.Store.Remove(merchantsProduct.First());
                merchant.Store.Add(editedProduct);

                var isMerchantUpdated = this.repositoryHandler.MerchantRepository.Update(merchant);
                if (!isMerchantUpdated) {
                    throw new DbContextException($"The {nameof(merchant)} has not been updated.");
                }

                await this.repositoryHandler.SaveAsync();
                return this.mapper.Map<ProductModel>(editedProduct);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<ProductModel> GetProduct(long productId)
        {
            try {
                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                return this.mapper.Map<ProductModel>(product);
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetMaxQuantityPerOrder(long productId, int quantity)
        {
            try {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.MaxQuantityPerOrder = quantity;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetMinQuantityPerOrder(long productId, int quantity)
        {
            try {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.MinQuantityPerOrder = quantity;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetPrice(long productId, decimal price)
        {
            try {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.Price = price;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetProductAvailabilityStatus(long productId, ProductAvailabilityStatus productAvailabilityStatus)
        {
            try {
                if (!Enum.IsDefined(typeof(ProductAvailabilityStatus), productAvailabilityStatus)) {
                    throw new ArgumentOutOfRangeException();
                }
                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.AvailabilityStatus = productAvailabilityStatus;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        public async Task<bool> SetTotalAvailableQuantity(long productId, int quantity)
        {
            try {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

                var product = await this.repositoryHandler.ProductRepository.GetSingleAsync(x => x.Id == productId)
                    ?? throw new EntityNotFoundException($"Product with ID '{productId}' not found.");

                product.TotalAvailableQuantity = quantity;

                var isProductUpdated = this.repositoryHandler.ProductRepository.Update(product);
                if (!isProductUpdated) {
                    throw new DbContextException($"The {nameof(ProductBaseEntity)} has not been updated.");
                }

                return await this.repositoryHandler.SaveAsync() > 0;
            }
            catch (Exception ex) {
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, ex.Message)}");
                throw;
            }
        }

        // TODO: Implement... Abstract this out, need to be used in the API model as well 
        public bool IsValidProductModel(ProductModel productModel)
        {
            return false;
        }
    }
}
