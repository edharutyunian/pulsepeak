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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.ShoppingCartOperationsTests
{
    public class ClearCartTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task ClearCart_ValidShoppingCart_ReturnsEmptyCart()
        {
            // Arrange
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

            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>() { productModel },
                ShippingAddress = null,
                TotalItemCount = 1
            };

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
                Products = new List<ProductBaseEntity>() {productEntity },
                TotalItemCount = 1
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(true);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await shoppingCartOperations.ClearCart(shoppingCartModel);

            // Assert
            Assert.NotNull(result); //Problem in the testable method, returns null
            Assert.Empty(result.Products);
            Assert.Equal(0, result.TotalItemCount);
        }


        [Fact]
        public async Task ClearCart_ShoppingCartNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                PaymentMethod = null,
                Products = new List<ProductModel>() { },
                ShippingAddress = null,
                TotalItemCount = 1
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync((ShoppingCartBaseEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.ClearCart(shoppingCartModel));
        }

        [Fact]
        public async Task ClearCart_UpdateShoppingCartFails_ThrowsDbContextException()
        {
            // Arrange
            

            var shoppingCartModel = new ShoppingCartModel { Id = 1 };

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>() {  },
                TotalItemCount = 1
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(false); //  update fails

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => shoppingCartOperations.ClearCart(shoppingCartModel));
        }

    }
}
