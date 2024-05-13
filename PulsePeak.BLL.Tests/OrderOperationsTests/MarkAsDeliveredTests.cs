using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.OrderOperationsTests
{
    public class MarkAsDeliveredTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task MarkAsDelivered_ValidOrderId_ReturnsTrue()
        {
            // Arrange
           
            var orderId = 1; // valid order ID

            var orderEntity = new OrderBaseEntity
            {
                Id = orderId,
                Customer = null,
                PaymentMethod = null,
                ShippingAddress = null,
                OrderDate = new DateTime(2024, 5, 12),
                ShoppingCart = null,
                Active = true,
                CustomerId = 1,
                DeliveryDate = new DateTime(2024, 5, 14),
                IsCanceled = false,
                IsDelivered = true,
                IsShipped = true,
                OrderNumber = 1,
                OrderStatus = Core.Enums.OrderPlacementStatus.Shipped,
                PaymentMethodId = 1,
                ShippingAddressId = 1,
                ShippingDate = new DateTime(2024, 5, 12),
                ShoppingCartId = 1
            };

            repositoryHandlerMock.Setup(rh => rh.OrderRepository.GetSingleAsync(It.IsAny<Expression<Func<OrderBaseEntity, bool>>>()))
                .ReturnsAsync(orderEntity);

            repositoryHandlerMock.Setup(rh => rh.OrderRepository.Update(It.IsAny<OrderBaseEntity>()))
                .Returns(true);

            var orderOperations = new OrderOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await orderOperations.MarkAsDelivered(orderId);

            // Assert
            Assert.True(result);
          
        }

        [Fact]
        public async Task MarkAsDelivered_OrderNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
          

            repositoryHandlerMock.Setup(rh => rh.OrderRepository.GetSingleAsync(It.IsAny<Expression<Func<OrderBaseEntity, bool>>>()))
                .ReturnsAsync((OrderBaseEntity)null);

            var orderOperations = new OrderOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => orderOperations.MarkAsDelivered(1));
        }

        [Fact]
        public async Task MarkAsDelivered_UpdateOrderFails_ThrowsDbContextException()
        {
            // Arrange
            

            repositoryHandlerMock.Setup(rh => rh.OrderRepository.GetSingleAsync(It.IsAny<Expression<Func<OrderBaseEntity, bool>>>()))
                .ReturnsAsync(new OrderBaseEntity()
                {
                    Id = 1,
                    Customer = null,
                    PaymentMethod = null,
                    ShippingAddress = null,
                    OrderDate = new DateTime(2024, 5, 12),
                    ShoppingCart = null,
                    Active = true,
                    CustomerId = 1,
                    DeliveryDate = new DateTime(2024, 5, 14),
                    IsCanceled = false,
                    IsDelivered = true,
                    IsShipped = true,
                    OrderNumber = 1,
                    OrderStatus = Core.Enums.OrderPlacementStatus.Shipped,
                    PaymentMethodId = 1,
                    ShippingAddressId = 1,
                    ShippingDate = new DateTime(2024, 5, 12),
                    ShoppingCartId = 1

                });

            repositoryHandlerMock.Setup(rh => rh.OrderRepository.Update(It.IsAny<OrderBaseEntity>()))
                .Returns(false); // update fails

            var orderOperations = new OrderOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => orderOperations.MarkAsDelivered(1));
        }
    }
}
