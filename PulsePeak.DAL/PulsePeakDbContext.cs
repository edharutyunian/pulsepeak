using Microsoft.EntityFrameworkCore;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Orders;

namespace PulsePeak.DAL
{
    public class PulsePeakDbContext : DbContext
    {
        public PulsePeakDbContext(DbContextOptions<PulsePeakDbContext> options) : base(options) { }

        DbSet<UserBaseEnttity> Users { get; set; }
        DbSet<CustomerEntity> Customers { get; set; }
        DbSet<MerchantEntity> Merchant { get; set; }
        DbSet<AddressBaseEntity> Addresses { get; set; }
        DbSet<PaymentMehodBaseEntity> PaymentMehods { get; set; }
        DbSet<CreditCardEntity> CreditCards { get; set; }
        DbSet<ProductBaseEntity> Products { get; set; }
        DbSet<ShoppingCartBaseEntity> ShoppingCarts { get; set; }
        DbSet<OrderBaseEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO [ED]: Implement
            base.OnModelCreating(modelBuilder);
        }
    }
}
