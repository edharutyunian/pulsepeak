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
    public class SetMaxQuantityPerOrderTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task SetMaxQuantityPerOrder_ValidInput_ReturnsTrue()
        {
            // Arrange


            var productId = 1; //  valid product ID
            var quantity = 10; //  valid quantity

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
            var result = await productOperations.SetMaxQuantityPerOrder(productId, quantity);

            // Assert
            Assert.True(result); // There's problem in BLL method, the test works correctly 

        }

        //This test is failing, don't know why, the method throws ArgumentOutOfRangeException
        [Fact]
        public async Task SetMaxQuantityPerOrder_InvalidQuantity_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            long productId = 1;
            int quantity = -1;
            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => productOperations.SetMaxQuantityPerOrder(productId, quantity));
        }

        [Fact]
        public async Task SetMaxQuantityPerOrder_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            long productId = 9999; //Invalid productId
            int quantity = 5;
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
             .ReturnsAsync((ProductBaseEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.SetMaxQuantityPerOrder(productId, quantity));
        }

        [Fact]
        public async Task SetMaxQuantityPerOrder_UpdateProductFails_ThrowsDbContextException()
        {
            // Arrange
            long productId = 1;
            int quantity = 5;

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(new ProductBaseEntity
                { 
                    Merchant = null, 
                    Title = "test"
                });

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false); //  update fails

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => productOperations.SetMaxQuantityPerOrder(productId, quantity));
        }

    }
}
