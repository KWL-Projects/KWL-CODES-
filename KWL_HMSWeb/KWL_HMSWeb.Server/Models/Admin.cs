using System.ComponentModel.DataAnnotations;

namespace KWL_HMSWeb.Server.Models
{
    public class Admin
    {
        [Key]
        public int user_id { get; set; }
        public string admin_Role { get; set; }
    }
}
