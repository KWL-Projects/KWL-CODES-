using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace KWL_HMSWeb.Server.Models
{
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int login_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;
    }
}
