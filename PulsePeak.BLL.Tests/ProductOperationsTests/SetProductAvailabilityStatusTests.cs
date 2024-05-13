using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Enums;
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
    public class SetProductAvailabilityStatusTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Theory]
        [InlineData(ProductAvailabilityStatus.Available)]
        [InlineData(ProductAvailabilityStatus.OutOfStock)]

        public async Task SetProductAvailabilityStatus_ValidInput_ReturnsTrue(ProductAvailabilityStatus status)
        {
            // Arrange

            var productId = 1; // valid product ID

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
            var result = await productOperations.SetProductAvailabilityStatus(productId, status);

            // Assert
            Assert.True(result); //problem in method return 

        }

        [Fact]
        public async Task SetProductAvailabilityStatus_InvalidStatus_ThrowsArgumentOutOfRangeException()
        {
            // Arrange


            var productId = 1; // valid product ID
            var status = (ProductAvailabilityStatus)100; //  invalid status

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => productOperations.SetProductAvailabilityStatus(productId, status));
        }

        [Fact]
        public async Task SetProductAvailabilityStatus_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            var productId = 9999; // invalid product ID
            var status = ProductAvailabilityStatus.Available;
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync((ProductBaseEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.SetProductAvailabilityStatus(productId, status));
        }

        [Fact]
        public async Task SetProductAvailabilityStatus_UpdateProductFails_ThrowsDbContextException()
        {
            // Arrange
            var productId = 1; // valid product ID
            var status = ProductAvailabilityStatus.Available; //valid status

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(new ProductBaseEntity
                {
                    Merchant = null,
                    Title = "Test"
                });


            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false); //  update fails

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => productOperations.SetProductAvailabilityStatus(productId, status));
        }
    }
}
