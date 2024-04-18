using Microsoft.EntityFrameworkCore.Storage;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.AddressRepositoryContracts;

namespace PulsePeak.Core.RepositoryContracts.RepositoryAbstraction
{
    public interface IRepositoryHandler
    {
        IUserRepository UserRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IMerchantRepository MerchantRepository { get; }
        IAddressRepository AddressRepository { get; }
        IBillingAddressRepository BillingAddressRepository { get; }
        IShippingAddressRepository ShippingAddressRepository { get; }
        IContactRepository ContactRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        ICreditCardRepository CreditCardRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }

        int Save();
        Task<int> SaveAsync();
        IDbContextTransaction CreateTransaction();
        Task<IDbContextTransaction> CreateTransactionAsync();
    }
}
