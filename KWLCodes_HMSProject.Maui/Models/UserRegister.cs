using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class UserRegister
    {
        public string username { get; set; }        // User's username
        public string password { get; set; }        // User's password
        public string user_firstname { get; set; }  // User's first name
        public string user_surname { get; set; }    // User's surname
        public string user_type { get; set; }       // User's role/type
    }
}
