using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Enums;
using PulsePeak.Core.Entities.Categories;
using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Products
{
    public class ProductBaseEntity : EntityBase, IProduct
    {
        [Required]
        [ForeignKey("Owner.Id")]
        public long UserId { get; set; }
        public required MerchantEntity Owner { get; set; } // required -- represents WHO is the owner of the Product || can also be named as Brand, but it really depends 

        [Required]
        [ForeignKey("Category.Id")]
        public long CategoryId { get; set; }
        [Required]
        public required ICategory Category { get; set; } // required -- but need to implement the ICategory stuff end-to-end

        public DateTime CreatedOn { get; set; } // defaults to the DateTime.Now or smth
        [Timestamp]
        [Column(TypeName = "datetime")]
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

        // TODO: [ED] Implement a Rating model and service
        public double ProductRate { get; set; } // defaults to 0 upon creation, validation -- [1.0<x>5.0], should be counted based on the count of rewies
        public int ReviewCount { get; set; } // total count of Reviews
        public int OrderCount { get; set; } // count of orders
        public ProductAvailabilityStatus AvailabilityStatus { get; set; } // defaults to Available upon creation
    }
}