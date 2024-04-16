using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Contacts
{
    public class ContactBaseEntity : EntityBase, IContact
    {
        public long UserId { get; set; }
        public UserBaseEnttity User { get; set; } // required 

        public string Value { get; set; } // required -- validation can be found in the BLL.Utils or similar for both EmailAddress and PhoneNumber
        public ContactType Type { get; set; } // required
    }
}
