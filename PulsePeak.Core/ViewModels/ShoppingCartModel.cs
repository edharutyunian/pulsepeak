using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;

namespace PulsePeak.Core.ViewModels
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel() => this.Products = new List<ProductModel>();

        public long Id { get; set; }
        public ICollection<ProductModel> Products { get; set; }
        public long CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public int TotalItemCount { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public PaymentMethodModel PaymentMethod { get; set; }
        public decimal TotalPrice => this.Products.Sum(x => x.Price * x.AddedQuantity);
    }
}