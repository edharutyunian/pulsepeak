using PulsePeak.Core.Enums;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;

namespace PulsePeak.Core.ViewModels
{
    public class OrderModel
    {
        public long Id { get; set; }
        public int OrderNumber { get; set; }
        public long CustomerId { get; set; }
        public CustomerModel Customer { get; set; } // Consider using a simplified customer model
        public long ShoppingCartId { get; set; }
        public ShoppingCartModel ShoppingCart { get; set; } // For detailed cart review
        public long ShippingAddressId { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public long PaymentMethodId { get; set; }
        public PaymentMethodModel PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderPlacementStatus OrderStatus { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsShipped { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsCanceled { get; set; }
    }
}
