using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Users;
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
    public class SetPaymentMethodTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task SetPaymentMethod_ValidInputs_ReturnsUpdatedShoppingCart()
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel
            {
                Id = 1,
                CustomerId = 1,
                PaymentMethod = new PaymentMethodModel
                {
                    Id = 1,
                    CardholderName = "Tatev Tshagharyan",
                    CardNumber = "154643212",
                    ExpirationMonth = 5,
                    ExpirationYear = 2026,
                    IsPrimary = true
                },
            };
            var paymentMethodId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };
            var paymentMethodEntity = new PaymentMehodBaseEntity
            {
                Id = 1,
                Active = true,
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true

            };
            var customerEntity = new CustomerEntity
            {
                Id = 1,
                PaymentMethods = new List<PaymentMehodBaseEntity> { paymentMethodEntity },
                EmailAddress = "test@gmail.com",
                FirstName = "Tatev",
                LastName = "Tshagharyan",
                Password = "5456411",
                PhoneNumber = "1234567890",
                UserName = "user",

            };

            var paymentMethodModel = new PaymentMethodModel
            {
                Id = 1,                
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.GetSingleAsync(It.IsAny<Expression<Func<PaymentMehodBaseEntity, bool>>>()))
                .ReturnsAsync(paymentMethodEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(true);
            mapperMock.Setup(m => m.Map<PaymentMethodModel>(paymentMethodEntity))
               .Returns(paymentMethodModel);
            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paymentMethodId, result.PaymentMethod.Id);
        }


        [Fact]
        public async Task SetPaymentMethod_ShoppingCartNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            

            var shoppingCartModel = new ShoppingCartModel { Id = 1 };
            var paymentMethodId = 1;



            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync((ShoppingCartBaseEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId));
        }

        [Fact]
        public async Task SetPaymentMethod_PaymentMethodNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1 };
            var paymentMethodId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.GetSingleAsync(It.IsAny<Expression<Func<PaymentMehodBaseEntity, bool>>>()))
                .ReturnsAsync((PaymentMehodBaseEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId));
        }

        [Fact]
        public async Task SetPaymentMethod_CustomerNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           
            var shoppingCartModel = new ShoppingCartModel { Id = 1 };
            var paymentMethodId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };

            var paymentMethodEntity = new PaymentMehodBaseEntity
            {
                Id = 1,
                Active = true,
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true

            };
            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.GetSingleAsync(It.IsAny<Expression<Func<PaymentMehodBaseEntity, bool>>>()))
                .ReturnsAsync(paymentMethodEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync((CustomerEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId));
        }

        [Fact]
        public async Task SetPaymentMethod_PaymentMethodNotAssociatedWithCustomer_ThrowsKeyNotFoundException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var paymentMethodId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };
            var paymentMethodEntity = new PaymentMehodBaseEntity
            {
                Id = 1,
                Active = true,
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true

            };
            var customerEntity = new CustomerEntity
            {
                Id = 1,
                PaymentMethods = new List<PaymentMehodBaseEntity> {  },
                EmailAddress = "test@gmail.com",
                FirstName = "Tatev",
                LastName = "Tshagharyan",
                Password = "5456411",
                PhoneNumber = "1234567890",
                UserName = "user",

            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.GetSingleAsync(It.IsAny<Expression<Func<PaymentMehodBaseEntity, bool>>>()))
                .ReturnsAsync(paymentMethodEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId));
        }

        [Fact]
        public async Task SetPaymentMethod_UpdateShoppingCartFails_ThrowsDbContextException()
        {
            // Arrange
            

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var paymentMethodId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };
            var paymentMethodEntity = new PaymentMehodBaseEntity
            {
                Id = 1,
                Active = true,
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true

            };
            var customerEntity = new CustomerEntity
            {
                Id = 1,
                PaymentMethods = new List<PaymentMehodBaseEntity> { paymentMethodEntity},
                EmailAddress = "test@gmail.com",
                FirstName = "Tatev",
                LastName = "Tshagharyan",
                Password = "5456411",
                PhoneNumber = "1234567890",
                UserName = "user",

            };
            var paymentMethodModel = new PaymentMethodModel
            {
                Id = 1,
                CardholderName = "Tatev Tshagharyan",
                CardName = "Visa",
                CardNumber = "12457889636541",
                Customer = null,
                CustomerId = 1,
                CVV = 122,
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsActive = true,
                IsPrimary = true
            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.GetSingleAsync(It.IsAny<Expression<Func<PaymentMehodBaseEntity, bool>>>()))
                .ReturnsAsync(paymentMethodEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(false);
            mapperMock.Setup(m => m.Map<PaymentMethodModel>(paymentMethodEntity))
              .Returns(paymentMethodModel);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => shoppingCartOperations.SetPaymentMethod(shoppingCartModel, paymentMethodId));
        }

    }
}
