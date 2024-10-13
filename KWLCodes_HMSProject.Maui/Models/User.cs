using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class User
    {
        public int UserId { get; set; }          // Primary Key
        public string UserFirstname { get; set; } // User's first name
        public string UserSurname { get; set; }    // User's surname
        public string UserType { get; set; }       // User's role/type
        public int LoginId { get; set; }           // Foreign Key
       // public virtual Login login_id { get; set; }   // Navigation property
    }

}
