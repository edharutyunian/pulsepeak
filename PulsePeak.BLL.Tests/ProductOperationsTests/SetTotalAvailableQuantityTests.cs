using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.ProductOperationsTests
{
    public class SetTotalAvailableQuantityTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task SetTotalAvailableQuantity_ValidInput_ReturnsTrue()
        {
            // Arrange
            

            var productId = 1; // Provide valid product ID
            var quantity = 100; // Provide valid quantity

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

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(true);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await productOperations.SetTotalAvailableQuantity(productId, quantity);

            // Assert
            Assert.True(result); //problem in method return 
            
        }

        [Fact]
        public async Task SetTotalAvailableQuantity_InvalidQuantity_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            
            var productId = 1; //  valid product ID
            var quantity = -100; //  invalid (negative) quantity

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => productOperations.SetTotalAvailableQuantity(productId, quantity));
        }

        [Fact]
        public async Task SetTotalAvailableQuantity_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var productId = 9999; //  invalid product ID
            var quantity = 100; //  valid (negative) quantity
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync((ProductBaseEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.SetTotalAvailableQuantity(productId, quantity));
        }

        [Fact]
        public async Task SetTotalAvailableQuantity_UpdateProductFails_ThrowsDbContextException()
        {
            // Arrange

            var productId = 1; // Provide valid product ID
            var quantity = 100; // Provide valid quantity
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(new ProductBaseEntity()
                {
                    Merchant= null,
                    Title = "Test"
                });

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false); //   update fails

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => productOperations.SetTotalAvailableQuantity(productId, quantity));
        }

    }
}
