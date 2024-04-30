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
        private readonly string errorMessage;

        public ProductOperations(ILogger logger, IRepositoryHandler repositoryHandler, IMapper mapper)
        {
            this.log = logger;
            this.repositoryHandler = repositoryHandler;
            this.mapper = mapper;
            this.errorMessage = string.Empty;
        }


        // TODO [Ed]: Implement
        public Task<ProductModel> AddProduct(long merchantId, ProductModel productModel)
        {
            throw new NotImplementedException();
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

        // TODO [Ed]: Implement
        public Task<ProductModel> EditProduct(ProductModel productModel)
        {
            throw new NotImplementedException();
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
    }
}
