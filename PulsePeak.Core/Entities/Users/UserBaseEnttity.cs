using System.ComponentModel.DataAnnotations;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Contacts;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    public class UserBaseEnttity : EntityBase, IUserAccount
    {
        [Required]
        public string FirstName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        [Required]
        public string LastName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string FullName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        [Required]
        public string UserName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        [Required]
        public string Password { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public IAddress BillingAddress { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public ICollection<IContact> Contacts { get; set; } // // required -- validation not implemented yet
        [Required]
        public UserType Type { get; set; } // required 
        public UserExecutionStatus ExecutionStatus { get; set; } // should be defaulted to UserExecutionStatus.NOTVERIFIED
        public byte[] ProfilePicture { get; set; } // optional

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }

        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public long MerchantId { get; set; }
        public MerchantEntity Merchant { get; set; }
    }
}