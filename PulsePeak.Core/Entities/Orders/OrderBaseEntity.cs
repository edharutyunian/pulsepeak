using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Orders
{
    [Table("Orders")]
    public class OrderBaseEntity : EntityBase, IOrder
    {
        [Required]
        [ForeignKey("Customer.Id")]
        public long CustomerId { get; set; }
        public required CustomerEntity Customer { get; set; }

        [Required]
        [ForeignKey("ShoppingCart.Id")]
        public long ShoppingCartId { get; set; }
        public required IShoppingCart ShoppingCart { get; set; }

        [Required]
        [ForeignKey("ShippingAddress.Id")]
        public long ShippingAddressId { get; set; }
        public required IAddress ShippingAddress { get; set; }

        [Required]
        [ForeignKey("PaymentMethod.Id")]
        public long PaymentMethodId { get; set; }
        [Required]
        public required IPaymentMethod PaymentMethod { get; set; } // required

        [Required]
        public int OrderNumber { get; set; } // Generatable

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderPlacementStatus OrderStatus { get; set; } // defaults to OrderPlacementStatus.Pendion upon Order creation
        public DateTime ShippingDate { get; set; } // let's just have something general, like +2 days after order is confirmed
        public DateTime DeliveryDate { get; set; } // let's just have something general, like +3 days after the ShippingDate


        public bool IsShipped { get; set; }
        public bool IsDelivered { get; set; }
    }
}
