using PulsePeak.Core.Enums.UserEnums;

namespace PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels
{
    public class CustomerPersonalInfoModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}
