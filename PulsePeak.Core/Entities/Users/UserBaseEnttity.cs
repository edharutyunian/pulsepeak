using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Contacts;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    public class UserBaseEnttity : EntityBase
    {
        public string FirstName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string LastName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string FullName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        public string UserName { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public string Password { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public IAddress BillingAddress { get; set; } // required -- validation can be found in the BLL.Utils or similar
        public ICollection<IContact> Contacts { get; set; } // // required -- validation not implemented yet
        public UserType Type { get; set; } // required 
        public UserExecutionStatus ExecutionStatus { get; set; } // should be defaulted to UserExecutionStatus.NOTVERIFIED
        public byte[] ProfilePicture { get; set; } // optional


        // QQ -> Tigran && Davit
        // Maybe this is better then directly inheriting, correct?
        public CustomerEntity Customer { get; set; }
        public MerchantEntity Merchant { get; set; }
    }
}