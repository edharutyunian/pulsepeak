using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Payments
{
    [Table("CreditCards")]
    public class CreditCardEntity : EntityBase, IPaymentMethod
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UserId { get; set; }
        public required IUserAccount User { get; set; }

        [Required]
        [ForeignKey("PaymentMehod.Id")]
        public long PaymentMehodId { get; set; }
        public required PaymentMehodBaseEntity PaymentMehod { get; set; }

        [Required]
        [Column(TypeName = "varchar(16)")]
        public required string CardNumber { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string HolderName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CardName { get; set; }

        [Required]
        [Range(1, 12)]
        [Column(TypeName = "tinyint")]
        public byte ExpirationMonth { get; set; }

        [Required]
        public byte ExpirationYear { get; set; } // validation -- should not be less than current date, and more than 5 years

        [Required]
        [Range(100, 999)]
        [Column(TypeName = "smallint")]
        public short CVV { get; set; } // validate that this is a 3digit number
    }
}
