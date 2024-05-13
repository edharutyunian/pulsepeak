using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.ViewModels.AddressModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.ShoppingCartOperationsTests
{
    public class SetShippingAddressTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task SetShippingAddress_ValidInputs_ReturnsUpdatedShoppingCart()
        {
            // Arrange


            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1, ShippingAddress = null };
            var shippingAddressId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

            };
            var shippingAddressEntity = new AddressBaseEntity
            {
                Id = 1,
                AddressType = Core.Enums.AddressType.Shipping,
                City = "Yerevan",
                Country = "Armenia",
                Customer = null,
                LocationName = "Test",
                Merchant = null,
                State = null,
                Street = null,
                ZipCode = "0028"

            };

            var shippingAddressModel = new AddressModel
            {
                Id = 1,
                City = "Yerevan",
                Country = "Armenia",
                LocationName = "Test",
                State = null,
                Street = null,
                ZipCode = "0028",
                AddressType = Core.Enums.AddressType.Shipping
                
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
                ShippingAddress = shippingAddressEntity,
                ShippingAddressId = shippingAddressId

            };


            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.AddressRepository.GetSingleAsync(It.IsAny<Expression<Func<AddressBaseEntity, bool>>>()))
                .ReturnsAsync(shippingAddressEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(true);
            mapperMock.Setup(m => m.Map<AddressModel>(shippingAddressEntity)).Returns(shippingAddressModel);
            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(shippingAddressId, result.ShippingAddress.Id);
        }



        [Fact]
        public async Task SetShippingAddress_ShoppingCartNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1 };
            var shippingAddressId = 1;

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync((ShoppingCartBaseEntity)null);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId));
        }

        [Fact]
        public async Task SetShippingAddress_ShippingAddressNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var shippingAddressId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

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
                ShippingAddressId = shippingAddressId

            };


            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.AddressRepository.GetSingleAsync(It.IsAny<Expression<Func<AddressBaseEntity, bool>>>()))
                .ReturnsAsync((AddressBaseEntity)null); // Shipping address not found

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId));
        }

        [Fact]
        public async Task SetShippingAddress_CustomerNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var shippingAddressId = 1;

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
                .ReturnsAsync(shoppingCartEntity); // Shopping cart found

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync((CustomerEntity)null); // Customer not found

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId));
        }

        [Fact]
        public async Task SetShippingAddress_CustomerNotAssociatedWithShippingAddress_ThrowsKeyNotFoundException()
        {
            // Arrange
            

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var shippingAddressId = 1;
            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

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

                        
            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId));
        }

        [Fact]
        public async Task SetShippingAddress_UpdateShoppingCartFails_ThrowsDbContextException()
        {
            // Arrange
           

            var shoppingCartModel = new ShoppingCartModel { Id = 1, CustomerId = 1 };
            var shippingAddressId = 1;

            var shoppingCartEntity = new ShoppingCartBaseEntity
            {
                Id = 1,
                Customer = null,
                CustomerId = 1,
                Products = new List<ProductBaseEntity>(),
                TotalItemCount = 1,
                Active = true,

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
                ShippingAddressId = 1

            };
            var shippingAddressEntity = new AddressBaseEntity
            {
                Id = 1,
                AddressType = Core.Enums.AddressType.Shipping,
                City = "Yerevan",
                Country = "Armenia",
                Customer = null,
                LocationName = "Test",
                Merchant = null,
                State = null,
                Street = null,
                ZipCode = "0028"

            };

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.GetSingleAsync(It.IsAny<Expression<Func<ShoppingCartBaseEntity, bool>>>()))
                .ReturnsAsync(shoppingCartEntity);

            repositoryHandlerMock.Setup(rh => rh.CustomerRepository.GetSingleAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                .ReturnsAsync(customerEntity);

            repositoryHandlerMock.Setup(rh => rh.AddressRepository.GetSingleAsync(It.IsAny<Expression<Func<AddressBaseEntity, bool>>>()))
                .ReturnsAsync(shippingAddressEntity);

            repositoryHandlerMock.Setup(rh => rh.ShoppingCartRepository.Update(It.IsAny<ShoppingCartBaseEntity>()))
                .Returns(false); // Update fails

            var shoppingCartOperations = new ShoppingCartOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => shoppingCartOperations.SetShippingAddress(shoppingCartModel, shippingAddressId));
        }
    }
}
