using Microsoft.EntityFrameworkCore;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Orders;
using System.Net.NetworkInformation;

namespace PulsePeak.DAL
{
    public class PulsePeakDbContext : DbContext
    {
        public PulsePeakDbContext(DbContextOptions<PulsePeakDbContext> options) : base(options) { }

        public DbSet<UserBaseEnttity> Users { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<MerchantEntity> Merchant { get; set; }
        public DbSet<AddressBaseEntity> Addresses { get; set; }
        public DbSet<PaymentMehodBaseEntity> PaymentMehods { get; set; }
        public DbSet<CreditCardEntity> CreditCards { get; set; }
        public DbSet<ProductBaseEntity> Products { get; set; }
        public DbSet<ShoppingCartBaseEntity> ShoppingCarts { get; set; }
        public DbSet<OrderBaseEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO [ED]: Implement
            base.OnModelCreating(modelBuilder);
        }
    }
}
