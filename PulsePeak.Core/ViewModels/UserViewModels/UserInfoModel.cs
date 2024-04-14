using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.Core.ViewModels.UserViewModels
{
    public class UserInfoModel
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
