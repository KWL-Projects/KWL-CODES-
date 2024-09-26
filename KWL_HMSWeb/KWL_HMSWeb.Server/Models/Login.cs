using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KWL_HMSWeb.Server.Models
{
    public class Login
    {
        [Key]
        public int login_id { get; set; }

        public string username { get; set; }
        public string password { get; set; }

        // Navigation property
        public virtual ICollection<User> Users { get; set; }
    }
}
