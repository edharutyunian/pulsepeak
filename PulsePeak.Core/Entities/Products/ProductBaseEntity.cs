using PulsePeak.Core.Entities.Categories;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Products
{
    public class ProductBaseEntity : EntityBase, IProduct
    {
        public MerchantEntity Owner { get; set; } // required -- represents WHO is the owner of the Product || can also be named as Brand, but it really depends 
        public ICategory Category { get; set; } // required -- but need to implement the ICategory stuff end-to-end
        public IUserAccount CreatedByID { get; set; } // required -- this is should be the MerchantEntity.ID 
        public DateTime CreatedOn { get; set; } // defaults to the DateTime.Now or smth
        public DateTime ModifiedOn { get; set; } // DB stuff -- Update by DateTime.Now if smth has been updated on product
        public string Title { get; set; } // required -- just a string with a validation of 200 characters
        public string Description { get; set; } // required -- just a string with a validation of 4000 characters 
        public double Price { get; set; } // required
        public int TotalQuantity { get; set; } // at lease 1 item required 
        public int MinQuantityPerOrder { get; set; } // at lease 1 
        public int MaxQuantityPerOrder { get; set; }// at lease 1 
        public double ProductRate { get; set; } // defaults to 0 upon creation, validation -- [1.0<x>5.0], should be counted based on the count of rewies
        public int ReviewCount { get; set; } // total count of Reviews
        public int OrderCount { get; set; } // count of orders
        public ProductAvailabilityStatus AvailabilityStatus { get; set; } // defaults to Available upon creation
    }
}