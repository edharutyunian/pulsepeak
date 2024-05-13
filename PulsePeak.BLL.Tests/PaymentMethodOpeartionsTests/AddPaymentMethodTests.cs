using AutoMapper;
using Castle.Core.Resource;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Payments;
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

namespace PulsePeak.BLL.Tests.PaymentMethodOpeartionsTests
{
    public class AddPaymentMethodTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task AddPaymentMethod_ValidPaymentMethod_AddsPaymentMethodSuccessfully()
        {
            // Arrange
           
            var customerId = 1;
            var paymentMethodModel = new PaymentMethodModel 
            { 
                Id = 1, 
                CustomerId = customerId,
                CardholderName= "Tatev Tshagharyan",
                CardNumber = "13543541132",
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsPrimary = true,
            };

            var customerEntity = new CustomerEntity
            {
                Id = 1,
                PaymentMethods = new List<PaymentMehodBaseEntity> { },
                EmailAddress = "test@gmail.com",
                FirstName = "Tatev",
                LastName = "Tshagharyan",
                Password = "5456411",
                PhoneNumber = "1234567890",
                UserName = "user",
                ShippingAddress = null,
                ShippingAddressId = 2

            };
            var paymentMethodBaseEntity = new PaymentMehodBaseEntity
                {
                Id = 1, 
                CustomerId = customerId,
                CardholderName = "Tatev Tshagharyan",
                CardNumber = "13543541132",
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsPrimary = true,
                Customer = null,
                CVV = 263
            };
            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.PaymentMethodRepository.AddPaymentMethod(customerId, paymentMethodModel))
                .Returns(paymentMethodModel); // Simulating successful addition
            mapperMock.Setup(m => m.Map<PaymentMehodBaseEntity>(paymentMethodModel)).Returns(paymentMethodBaseEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.Update(customerEntity)).Returns(true);
            var paymentMethodOperations = new PaymentMethodOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await paymentMethodOperations.AddPaymentMethod(customerId, paymentMethodModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paymentMethodModel, result); //? problems in the testable method? returns null 
        }



        [Fact]
        public async Task AddPaymentMethod_CustomerNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           

            var customerId = 1;
            var paymentMethodModel = new PaymentMethodModel
            {
                Id = 1,
                CustomerId = customerId,
                CardholderName = "Tatev Tshagharyan",
                CardNumber = "13543541132",
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsPrimary = true,
            };

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync((CustomerEntity)null); // Customer not found

            var paymentMethodOperatiosn = new PaymentMethodOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => paymentMethodOperatiosn.AddPaymentMethod(customerId, paymentMethodModel));
        }

        [Fact]
        public async Task AddPaymentMethod_CustomerIdMismatch_ThrowsRegistrationException()
        {
            // Arrange
            

            var customerId = 1;
            var paymentMethodModel = new PaymentMethodModel
            {
                Id = 1,
                CustomerId = customerId +1,
                CardholderName = "Tatev Tshagharyan",
                CardNumber = "13543541132",
                ExpirationMonth = 5,
                ExpirationYear = 2026,
                IsPrimary = true,
            };
            // CustomerId doesn't match

            var customerEntity = new CustomerEntity
            {
                Id = customerId,
                PaymentMethods = new List<PaymentMehodBaseEntity> { },
                EmailAddress = "test@gmail.com",
                FirstName = "Tatev",
                LastName = "Tshagharyan",
                Password = "5456411",
                PhoneNumber = "1234567890",
                UserName = "user",
                ShippingAddress = null,
                ShippingAddressId = 2

            };

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            var paymentMethodOperatiosn = new PaymentMethodOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<RegistrationException>(() => paymentMethodOperatiosn.AddPaymentMethod(customerId, paymentMethodModel));
        }
    }
}
