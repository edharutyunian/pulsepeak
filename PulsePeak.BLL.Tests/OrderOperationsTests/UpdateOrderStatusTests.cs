using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Enums;
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
    public class UpdateOrderStatusTests
    {
         public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Theory]
        [InlineData(OrderPlacementStatus.Delivered)]
        [InlineData(OrderPlacementStatus.Processing)]
        [InlineData(OrderPlacementStatus.Shipped)]
        [InlineData(OrderPlacementStatus.Canceled)]
        public async Task UpdateOrderStatus_ValidOrderPlacementStatus_ReturnsTrue(OrderPlacementStatus status)
        {
            // Arrange
            
            var orderId = 1; //  valid order ID

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
                IsCanceled = true,
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
            var result = await orderOperations.UpdateOrderStatus(orderId, status);

            // Assert
            Assert.True(result);
           
        }

        [Fact]
        public async Task UpdateOrderStatus_InvalidOrderPlacementStatus_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            long orderId = 1;
            

            var orderOperations = new OrderOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => orderOperations.UpdateOrderStatus(orderId, (OrderPlacementStatus)100));
        }

        [Fact]
        public async Task UpdateOrderStatus_OrderNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            long orderId = 999999; // invalid order id 
            repositoryHandlerMock.Setup(rh => rh.OrderRepository.GetSingleAsync(It.IsAny<Expression<Func<OrderBaseEntity, bool>>>()))
                .ReturnsAsync((OrderBaseEntity)null);

            var orderOperations = new OrderOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => orderOperations.UpdateOrderStatus(orderId, OrderPlacementStatus.Canceled));
        }

        [Fact]
        public async Task UpdateOrderStatus_UpdateOrderFails_ThrowsDbContextException()
        {
            // Arrange

            long orderId = 1;
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
                    IsDelivered = false,
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
            await Assert.ThrowsAsync<DbContextException>(() => orderOperations.UpdateOrderStatus(orderId, OrderPlacementStatus.Delivered));
        }

    }
}
