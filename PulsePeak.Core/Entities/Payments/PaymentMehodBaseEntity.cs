using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Payments
{
    [Table("PaymentMethods")]
    public class PaymentMehodBaseEntity : EntityBase, IPaymentMethod
    {
        [Required]
        [ForeignKey("Customer.Id")]
        public long CustomerId { get; set; }
        public required CustomerEntity Customer { get; set; }

        [Required]
        [Column(TypeName = "varchar(16)")]
        public required string CardNumber { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string CardholderName { get; set; }

        [Required]
        [Range(1, 12)]
        [Column(TypeName = "tinyint")]
        public required byte ExpirationMonth { get; set; }

        [Required]
        public required short ExpirationYear { get; set; } // validation -- should not be less than current date, and more than 5 years

        [Required]
        [Range(100, 999)]
        [Column(TypeName = "smallint")]
        public required short CVV { get; set; } // validate that this is a 3digit number

        [Column(TypeName = "varchar(50)")]
        public string? CardName { get; set; }

        [Required]
        public required bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
    }
}
