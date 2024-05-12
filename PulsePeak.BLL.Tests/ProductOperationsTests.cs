
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PulsePeak.BLL.Operations;
using PulsePeak.Core.BLLOperationContracts;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;
using System.Linq.Expressions;

namespace PulsePeak.BLL.Tests
{
    public class ProductOperationsTests
    {
        public readonly Mock<IProductOperations> productOperationsMock = new();
        public readonly Mock<ILogger> loggerMock = new();
        public readonly Mock<IRepositoryHandler> repositoryHandlerMock = new();
        public readonly Mock<IMapper> mapperMock = new();

        [Fact]
        public async Task AddProduct_ValidInput_ReturnsProduct()
        {
            // Arrange           
            long merchantId = 1;
            var productModel = new ProductModel()
            {
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

            var addedProduct = new ProductModel
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

            var merchant = new MerchantEntity
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
                IsDeleted = false

            };

            repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<MerchantEntity, bool>>>()))
                .ReturnsAsync(merchant);

            repositoryHandlerMock.Setup(rh => rh.ProductRepository.AddProduct(It.IsAny<long>(), It.IsAny<ProductModel>()))
                .Returns(addedProduct);
            productOperationsMock.Setup(po => po.IsValidProductModel(productModel)).Returns(true);
            var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await productOperations.AddProduct(merchantId, productModel);

            // Assert
            Assert.NotNull(result);

        }

        //    [Fact]
        //    public async Task AddProduct_InvalidProductModel_ThrowsRegistrationException()
        //    {
        //        // Arrange
        //        var loggerMock = new Mock<ILogger>();
        //        var repositoryHandlerMock = new Mock<IRepositoryHandler>();
        //        var mapperMock = new Mock<IMapper>();

        //        var productModel = new ProductModel { /* Populate with invalid data */ };

        //        var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

        //        // Act & Assert
        //        await Assert.ThrowsAsync<RegistrationException>(() => productOperations.AddProduct(1 /* Provide valid merchant ID */, productModel));
        //    }

        //    [Fact]
        //    public async Task AddProduct_MerchantNotFound_ThrowsEntityNotFoundException()
        //    {
        //        // Arrange
        //        var loggerMock = new Mock<ILogger>();
        //        var repositoryHandlerMock = new Mock<IRepositoryHandler>();
        //        var mapperMock = new Mock<IMapper>();

        //        var productModel = new ProductModel { MerchantId = 999 /* Provide non-existing merchant ID */, /* other properties */ };

        //        repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<Merchant, bool>>>()))
        //            .ReturnsAsync((Merchant)null);

        //        var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

        //        // Act & Assert
        //        await Assert.ThrowsAsync<EntityNotFoundException>(() => productOperations.AddProduct(999 /* Provide non-existing merchant ID */, productModel));
        //    }

        //    [Fact]
        //    public async Task AddProduct_MerchantIdMismatch_ThrowsRegistrationException()
        //    {
        //        // Arrange
        //        var loggerMock = new Mock<ILogger>();
        //        var repositoryHandlerMock = new Mock<IRepositoryHandler>();
        //        var mapperMock = new Mock<IMapper>();

        //        var productModel = new ProductModel { MerchantId = 1 /* Provide valid merchant ID */, /* other properties */ };

        //        repositoryHandlerMock.Setup(rh => rh.MerchantRepository.GetSingleAsync(It.IsAny<Expression<Func<Merchant, bool>>>()))
        //            .ReturnsAsync(new Merchant { Id = 2 /* Provide different merchant ID */, /* other properties */ });

        //        var productOperations = new ProductOperations(loggerMock.Object, repositoryHandlerMock.Object, mapperMock.Object);

        //        // Act & Assert
        //        await Assert.ThrowsAsync<RegistrationException>(() => productOperations.AddProduct(1 /* Provide valid merchant ID */, productModel));
        //    }
        //}
    }
}