using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.ShoppingCart
{
    public class ShoppingCartBaseEntity : EntityBase, IShoppingCart
    {
        public int TotalItemCount { get; set; } // update upon adding an IProduct to thee ShoppingCart
        public Dictionary<IProduct, bool> Products { get; set; } // bool represents if the product is selected for checkout
        public CustomerEntity Customer { get; set; } 

        // anything else?
    }
}