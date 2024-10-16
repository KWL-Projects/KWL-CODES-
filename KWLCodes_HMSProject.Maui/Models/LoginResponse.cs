using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public string TokenError { get; set; }
    }
}