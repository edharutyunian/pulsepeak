using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.AddressModels;

namespace PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels
{
    public class MerchantModel
    {
        public required string CompanyName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }

        public long BillingAddressId { get; set; }
        public AddressModel BillingAddress { get; set; }

        public OrganizationType OrganizationType { get; set; } // required

        public ICollection<ProductModel>? Store { get; set; } // can be disregarded upon User creation | required only for checking out 

        public UserExecutionStatus ExecutionStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
    }
}
