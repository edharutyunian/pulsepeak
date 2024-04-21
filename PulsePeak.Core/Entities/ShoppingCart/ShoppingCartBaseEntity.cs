using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Products;

namespace PulsePeak.Core.Entities.ShoppingCart
{
    [Table("ShoppingCarts")]
    public class ShoppingCartBaseEntity : EntityBase, IShoppingCart
    {
        [Required]
        public int TotalItemCount { get; set; } // update upon adding an IProduct to thee ShoppingCart
        [Required]
        public required ICollection<IProduct> Products { get; set; }

        [Required]
        [ForeignKey("Customer.Id")]
        public long CustomerId { get; set; }
        [Required]
        public required CustomerEntity Customer { get; set; }

        // anything else?
    }
}