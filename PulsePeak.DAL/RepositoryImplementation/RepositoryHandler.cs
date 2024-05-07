using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.PaymentRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.EntityRepositoryContracts.UserRepositoryContracts;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;

namespace PulsePeak.DAL.RepositoryImplementation
{
    public class RepositoryHandler : IRepositoryHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly PulsePeakDbContext DbContext;

        public RepositoryHandler(IServiceProvider serviceProvider, PulsePeakDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.DbContext = dbContext;
        }

        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IMerchantRepository merchantRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;


        public IUserRepository UserRepository => this.userRepository ?? this.serviceProvider.GetRequiredService<IUserRepository>();
        public ICustomerRepository CustomerRepository => this.customerRepository ?? this.serviceProvider.GetRequiredService<ICustomerRepository>();
        public IMerchantRepository MerchantRepository => this.merchantRepository ?? this.serviceProvider.GetRequiredService<IMerchantRepository>();
        public IAddressRepository AddressRepository => this.addressRepository ?? this.serviceProvider.GetRequiredService<IAddressRepository>();
        public IPaymentMethodRepository PaymentMethodRepository => this.paymentMethodRepository ?? this.serviceProvider.GetRequiredService<IPaymentMethodRepository>();
        public IProductRepository ProductRepository => this.productRepository ?? this.serviceProvider.GetRequiredService<IProductRepository>();
        public IOrderRepository OrderRepository => this.orderRepository ?? this.serviceProvider.GetRequiredService<IOrderRepository>();
        public IShoppingCartRepository ShoppingCartRepository => this.shoppingCartRepository ?? this.serviceProvider.GetRequiredService<IShoppingCartRepository>();


        public int Save() => this.DbContext.SaveChanges();

        public Task<int> SaveAsync() => this.DbContext.SaveChangesAsync();

        public IDbContextTransaction CreateTransaction() => this.DbContext.Database.BeginTransaction();

        public Task<IDbContextTransaction> CreateTransactionAsync() => this.DbContext.Database.BeginTransactionAsync();
    }
}
