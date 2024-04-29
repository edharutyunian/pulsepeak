using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.Entities.Users
{
    [Table("Users")]
    public class UserBaseEnttity : EntityBase, IUserAccount
    {
        public CustomerEntity Customer { get; set; }
        public MerchantEntity Merchant { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string LastName { get; set; }

        public string FullName {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        [Required]
        [Column("Username", TypeName = "varchar(20)")]
        public required string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public required string Password { get; set; }

        [Required]
        public required UserType Type { get; set; }

        public UserExecutionStatus ExecutionStatus { get; set; }

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
    }
}