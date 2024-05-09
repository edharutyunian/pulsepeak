using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.AddressModels;

namespace PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels
{
    public class CustomerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }

        public long BillingAddressId { get; set; }
        public AddressModel BillingAddress { get; set; }

        public long ShippingAddressId { get; set; }
        public AddressModel ShippingAddress { get; set; }

        public DateTime BirthDate { get; set; }
        public Gender? Gender { get; set; }

        // TODO [ED]: update to IDictionarry<string, IPaymentMethod>
        // I guess it would be better to use IDictionarry<string, IPaymentMethod> with 'string' as a cardNumber to avoid any possible duplicate cards
        public ICollection<PaymentMethodModel?> PaymentMethods { get; set; }
        public PaymentMethodModel? PrimaryPaymentMethod => PaymentMethods.FirstOrDefault(x => x.IsActive);
        public ICollection<OrderModel>? Orders { get; set; }

        public UserExecutionStatus ExecutionStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
    }
}