using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.ViewModels
{
    public class ProductModel
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // defaults to the DateTime.Now or smth
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int AddedQuantity { get; set; }
        public int TotalAvailableQuantity { get; set; }
        public int MinQuantityPerOrder { get; set; }
        public int MaxQuantityPerOrder { get; set; }

        public ProductAvailabilityStatus AvailabilityStatus { get; set; } // defaults to Available upon creation

        public long MerchantId { get; set; }
        public required MerchantEntity Merchant { get; set; }
    }
}
