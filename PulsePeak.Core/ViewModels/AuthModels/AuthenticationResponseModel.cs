using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.Core.ViewModels.AuthModels
{
    public class AuthenticationResponseModel
    {
        public string UserName { get; set; }
        public DateTime AuthenticationDate { get; set; } // not sure if this should be a DateTime, but it's fine I guess 
        public string Token { get; set; }
    }
}
