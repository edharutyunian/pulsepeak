using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Products
{
    [Table("Products")]
    public class ProductBaseEntity : EntityBase, IProduct
    {
        [Required]
        [ForeignKey("User.Id")]
        public long UserId { get; set; }
        public required MerchantEntity User { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // defaults to the DateTime.Now or smth

        [Timestamp]
        [Column(TypeName = "datetime2")]
        public DateTime ModifiedOn { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public required string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [Required]
        [DefaultValue(1)]
        public int TotalAvailableQuantity { get; set; }

        [Required]
        [DefaultValue(1)]
        public int MinQuantityPerOrder { get; set; }

        [Required]
        [DefaultValue(1)]
        public int MaxQuantityPerOrder { get; set; }

        public ProductAvailabilityStatus AvailabilityStatus { get; set; } // defaults to Available upon creation

        public int OrderCount { get; set; } // total count of orders; increment on each order 
    }
}