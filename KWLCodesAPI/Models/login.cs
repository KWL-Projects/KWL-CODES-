using System.ComponentModel.DataAnnotations;

namespace KWLCodesAPI.Models
{
    public class Login
    {
        [Key]
        public int login_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
