using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.ShoppingCartOperationsTests
{
    public class AddProductToCartTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task AddProductToCart_ValidInputs_ReturnsShoppingCartModel()
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>(),
                ShippingAddress = null,
                TotalItemCount = 1
            };
            var productId = 1;
            var quantity = 1;

            var productEntity = new ProductBaseEntity()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Tshirt",
                Description = "Black",
                Price = 1200M,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(new ShoppingCartBaseEntity()
                { Customer = null, Products = null });

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(true);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(true);

            mapperMock.Setup(m => m.Map<ProductModel>(productEntity))
                .Returns(new ProductModel
                {
                    Id = 1,
                    CreatedOn = DateTime.UtcNow,
                    Title = "Tshirt",
                    Description = "Black",
                    Price = 1200M,
                    TotalAvailableQuantity = 20,
                    MinQuantityPerOrder = 1,
                    MaxQuantityPerOrder = 10,
                    AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                    MerchantId = 1,
                    Merchant = null
                });

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await shoppingCartOperations.AddProductToCart(shoppingCartModel, productId, quantity);

            // Assert
            Assert.NotNull(result);

        }

        [Theory]
        [InlineData(0)] // Zero quantity
        [InlineData(-1)] // Negative quantity
        public async Task AddProductToCart_InvalidQuantity_ThrowsArgumentOutOfRangeException(int quantity)
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>(),
                ShippingAddress = null,
                TotalItemCount = 1
            };
            var productId = 1;

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => shoppingCartOperations.AddProductToCart(shoppingCartModel, productId, quantity));
        }



        [Fact]
        public async Task AddProductToCart_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>(),
                ShippingAddress = null,
                TotalItemCount = 1
            };
            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1
            };
            var productId = 1; //invalid id 
            var quantity = 1;

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync((ProductBaseEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.AddProductToCart(shoppingCartModel, productId, quantity));
        }

        [Fact]
        public async Task AddProductToCart_UpdateProductFails_ThrowsDbContextException()
        {
            // Arrange
          

            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>(),
                ShippingAddress = null,
                TotalItemCount = 1
            };
            var productId = 1;
            var quantity = 1;

            var productEntity = new ProductBaseEntity()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Tshirt",
                Description = "Black",
                Price = 1200M,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };
            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1
            };
            var productModel = new ProductModel()
            {
                CreatedOn = DateTime.UtcNow,
                Title = "Top",
                Description = "Cropped",
                Price = 1335.54M,
                AddedQuantity = 1,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };

            mapperMock.Setup(m => m.Map<ProductModel>(productEntity)).Returns(productModel);
            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
               .ReturnsAsync(shoppingCartEntity);
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false); // update fails

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => shoppingCartOperations.AddProductToCart(shoppingCartModel, productId, quantity));
        }

        [Fact]
        public async Task AddProductToCart_UpdateShoppingCartFails_ThrowsDbContextException()
        {
            // Arrange

            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>(),
                ShippingAddress = null,
                TotalItemCount = 1
            };
            var productId = 1;
            var quantity = 1;

            var productEntity = new ProductBaseEntity()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Tshirt",
                Description = "Black",
                Price = 1200M,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(new ShoppingCartBaseEntity()
                { Customer =null, Products = new List<ProductBaseEntity>()});

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(true);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(false); // update fails

            mapperMock.Setup(m => m.Map<ProductModel>(productEntity))
                .Returns(new ProductModel { Title = "Test", Merchant = null });

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => shoppingCartOperations.AddProductToCart(shoppingCartModel, productId, quantity));
        }

    }
}
