using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Orders
{
    public class OrderBaseEntity : EntityBase, IOrder
    {
        public int OrderNumber { get; set; } // Generatable? 
        public DateTime OrderDate { get; set; } // defaults to DateTime.Now upon Order creation
        public OrderPlacementStatus OrderStatus { get; set; } // defaults to OrderPlacementStatus.Pendion upon Order creation
        public DateTime ShippingDate { get; set; } // let's just have something general, like +2 days after order is confirmed
        public DateTime DeliveryDate { get; set; } // let's just have something general, like +3 days after the ShippingDate
        public IShoppingCart ShoppingCart { get; set; } // required
        public IAddress ShippingAddress { get; set; } // required
        public IPaymentMethod PaymentMethod { get; set; } // required
    }
}
