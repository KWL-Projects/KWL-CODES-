using System.ComponentModel.DataAnnotations;

namespace KWL_HMSWeb.Server.Models
{
    public class Login
    {
        [Key]
        public int login_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}