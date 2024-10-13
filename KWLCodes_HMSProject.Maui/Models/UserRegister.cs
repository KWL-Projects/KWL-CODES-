using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class UserRegister
    {
        public string Username { get; set; }        // User's username
        public string Password { get; set; }        // User's password
        public string UserFirstname { get; set; }   // User's first name
        public string UserSurname { get; set; }      // User's surname
        public string UserType { get; set; }         // User's role/type
    }

}
