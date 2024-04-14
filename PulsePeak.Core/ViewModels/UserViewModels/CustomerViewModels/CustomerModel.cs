using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels
{
    public class CustomerModel
    {
        public UserModel User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}