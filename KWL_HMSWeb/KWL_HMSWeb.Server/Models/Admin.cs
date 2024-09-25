using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Admin
    {
        [Key, ForeignKey("User")]
        public int user_id { get; set; }

        public string admin_Role { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}