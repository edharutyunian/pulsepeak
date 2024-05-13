using PulsePeak.Core.Enums.UserEnums;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;

namespace PulsePeak.Core.BLLOperationContracts
{
    public interface ICustomerOperations
    {
        Task<CustomerRegistrationResponseModel> CustomerRegistration(CustomerRegistrationRequestModel customerRegistrationRequest);
        Task<CustomerModel> EditCustomerInfo(CustomerModel customerModel);
        Task<AddressModel> GetCustomerBillingAddress(int customerId);
        Task<AddressModel> GetCustomerShippingAddress(int customerId);
        Task<CustomerModel> GetCustomer(long customerId);
        Task<CustomerModel> GetCustomer(string username);
        Task<bool> IsActive(long customerId);
        Task<bool> IsActive(string username);
        Task<UserExecutionStatus> GetCustomerExecutionStatus(long customerId);
        Task<UserExecutionStatus> GetCustomerExecutionStatus(string username);
        Task SetCustomerExecutionStatus(long customerId, UserExecutionStatus status);
        Task SetCustomerExecutionStatus(string username, UserExecutionStatus status);
    }
}
