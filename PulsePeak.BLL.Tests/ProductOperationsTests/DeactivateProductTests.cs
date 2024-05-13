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
    public class DeactivateProductTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();


        [Fact]
        public async Task DeactivateProduct_ProductExists_DeactivatesProduct()
        {
            // Arrange
            var productId = 1;
            var product = new ProductBaseEntity
            {
                Id = productId,
                Active = true,
                Merchant = null,
                Title = "Title",
            };

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(product);
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(true);
            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            await productOperations.DeactivateProduct(productId);

            // Assert
            Assert.False(product.Active);

        }

        [Fact]
        public async Task DeactivateProduct_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            var productId = 999;

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync((ProductBaseEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.DeactivateProduct(productId));
        }

        [Fact]
        public async Task DeactivateProduct_UpdateFails_ThrowsDbContextException()
        {
            // Arrange
            var productId = 1;
            var product = new ProductBaseEntity
            {
                Id = productId,
                Active = true,
                Merchant = null,
                Title = "Title",
            };

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(product);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => productOperations.DeactivateProduct(productId));
        }


    }
}
