using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.BLL.Tests.ProductOperationsTests
{
    public class EditProductTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task EditProduct_ValidInput_ProductEditedSuccessfully()
        {
            // Arrange

            var productModel = new ProductModel()
            {
                Id = 1,
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
            var merchantEntity = new MerchantEntity
            {
                Id = 1,
                BillingAddressId = 1,
                CompanyName = "Test",
                FirstName = "Test1",
                LastName = "Test2",
                UserName = "Test3",
                Password = "kldjfe",
                EmailAddress = "taltj@gmail.com",
                PhoneNumber = "1234567890",
                OrganizationType = Core.Enums.UserEnums.OrganizationType.PrivateCorporation,
                ExecutionStatus = Core.Enums.UserEnums.UserExecutionStatus.ACTIVE,
                IsActive = true,
                IsBlocked = false,
                IsDeleted = false,
                Store = new List<ProductBaseEntity> { productEntity }

            };

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<MerchantEntity, bool>>>()))
                .ReturnsAsync(merchantEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(true);

            repositoryHandlerMock.Setup(rh => rh.MerchantRepository.Update(It.IsAny<MerchantEntity>()))
                .Returns(true);

            mapperMock.Setup(m => m.Map<ProductBaseEntity>(It.IsAny<ProductModel>()))
                .Returns(productEntity);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await productOperations.EditProduct(productModel); // fails on return this.mapper.Map<ProductModel>(editedProduct); (returns null)

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id == productModel.Id);
            Assert.True(result.MerchantId == productModel.MerchantId);
            Assert.True(result.AvailabilityStatus == productModel.AvailabilityStatus);
            Assert.True(result.AddedQuantity == productModel.AddedQuantity);
            Assert.True(result.CreatedOn == productModel.CreatedOn);
            Assert.True(result.Description == productModel.Description);
            Assert.True(result.MaxQuantityPerOrder == productModel.MaxQuantityPerOrder);
            Assert.True(result.MinQuantityPerOrder == productModel.MinQuantityPerOrder);
            Assert.True(result.Price == productModel.Price);
            Assert.True(result.Title == productModel.Title);
        }

        [Fact]
        public async Task EditProduct_InvalidProductModel_ThrowsRegistrationException()
        {
            // Arrange

            var productModel = new ProductModel()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Top",
                Description = "Cropped",
                Price = -50, // Invalid input
                AddedQuantity = 1,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<RegistrationException>(() => productOperations.EditProduct(productModel));
        }

        [Fact]
        public async Task EditProduct_ProductNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange


            var productModel = new ProductModel() //Non-existing productId, Valid MerchantId
            {
                Id = 999999,
                CreatedOn = DateTime.UtcNow,
                Title = "Top",
                Description = "Cropped",
                Price = -50, // Invalid input
                AddedQuantity = 1,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 1,
                Merchant = null
            };
            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync((ProductBaseEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.EditProduct(productModel));
        }

        [Fact]
        public async Task EditProduct_MerchantNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            var productModel = new ProductModel() //Non-existing merchantId, Valid ProductId
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Top",
                Description = "Cropped",
                Price = -50, // Invalid input
                AddedQuantity = 1,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 99999999,
                Merchant = null
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

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<MerchantEntity, bool>>>()))
                .ReturnsAsync((MerchantEntity)null);

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.EditProduct(productModel));
        }



        [Fact]
        public async Task EditProduct_UpdateProductFails_ThrowsDbContextException()
        {
            // Arrange


            var productModel = new ProductModel() //Valid productId, valid merchantId
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow,
                Title = "Top",
                Description = "Cropped",
                Price = -50, // Invalid input
                AddedQuantity = 1,
                TotalAvailableQuantity = 20,
                MinQuantityPerOrder = 1,
                MaxQuantityPerOrder = 10,
                AvailabilityStatus = Core.Enums.ProductAvailabilityStatus.Available,
                MerchantId = 99999999,
                Merchant = null
               
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
                Merchant = new MerchantEntity
                {
                    BillingAddressId = 1,
                    CompanyName = "Test",
                    FirstName = "Test1",
                    LastName = "Test2",
                    UserName = "Test3",
                    Password = "kldjfe",
                    EmailAddress = "taltj@gmail.com",
                    PhoneNumber = "1234567890",
                    OrganizationType = Core.Enums.UserEnums.OrganizationType.PrivateCorporation,
                    ExecutionStatus = Core.Enums.UserEnums.UserExecutionStatus.ACTIVE,
                    IsActive = true,
                    IsBlocked = false,
                    IsDeleted = false,
                }
            };

            var merchantEntity = new MerchantEntity
            {
                Id = 1,
                BillingAddressId = 1,
                CompanyName = "Test",
                FirstName = "Test1",
                LastName = "Test2",
                UserName = "Test3",
                Password = "kldjfe",
                EmailAddress = "taltj@gmail.com",
                PhoneNumber = "1234567890",
                OrganizationType = Core.Enums.UserEnums.OrganizationType.PrivateCorporation,
                ExecutionStatus = Core.Enums.UserEnums.UserExecutionStatus.ACTIVE,
                IsActive = true,
                IsBlocked = false,
                IsDeleted = false,
                Store = new List<ProductBaseEntity> { productEntity }
            };

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.GetSingleAsync(It.IsAny<Expression<Func<ProductBaseEntity, bool>>>()))
                .ReturnsAsync(productEntity);

            repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<MerchantEntity, bool>>>()))
                .ReturnsAsync(merchantEntity);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.Update(It.IsAny<ProductBaseEntity>()))
                .Returns(false); // Indicate that update fails

            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbContextException>(() => productOperations.EditProduct(productModel));
        }

       
    }
}
