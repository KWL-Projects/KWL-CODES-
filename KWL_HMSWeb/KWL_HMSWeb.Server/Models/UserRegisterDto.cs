using System.ComponentModel.DataAnnotations;

namespace KWL_HMSWeb.Server.Models
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [MaxLength(50)]
        public string user_firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string user_surname { get; set; }

        [Required]
        public string user_type { get; set; }
    }
}
