using PulsePeak.Core.Entities.Users;

namespace PulsePeak.Core.Entities.Categories
{
    public class CategoryBaseEntity : EntityBase, ICategory
    {
        public string Name { get; set; } // required -- less than 50 chars
        public string? Description { get; set; } // optional -- less than 200 chars
        public long ParentID { get; set; } // this should be used for having a sub-categories
        public byte[] Image { get; set; } // optional 
        public DateTime CreatedOn { get; set; } // defaults upon Category creation {DateTime.Now}
        public DateTime ModifiedOn { get; set; } // updates upon last modification 
        public MerchantEntity CreatedBy { get; set; } // required -- tied to the MerchantEntity's account 
    }
}
