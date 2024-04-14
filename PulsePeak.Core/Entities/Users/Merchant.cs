using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Enums.UserEnums;
using System.ComponentModel.DataAnnotations;

namespace PulsePeak.Core.Entities.Users
{
    public class Merchant : UserBaseEnttity, IUserAccount
    {
        public string CompanyName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string? POCName { get; set; } // defaults to the UserBaseEnttity.FullName
        public OrganizationType OrganizationType { get; set; } // required
        public ICollection<IProduct>? Store { get; set; } // can be disregarded upon User creation | required only for checking out 
    }
}
