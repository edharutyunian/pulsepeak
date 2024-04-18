using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Enums;

namespace PulsePeak.Core.Entities.Contacts
{
    public class ContactBaseEntity : EntityBase, IContact
    {
        [Required]
        [ForeignKey("Uers.Id")]
        public long UserId { get; set; }
        public required UserBaseEnttity User { get; set; }

        [Required]
        [Column(TypeName = "varchar(130)")]
        public required string Value { get; set; }
        public ContactType Type { get; set; } // required
    }
}
