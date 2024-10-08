using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace KWL_HMSWeb.Server.Models
{
    public class Login
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int login_id { get; set; } // Primary key

        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;

        // Navigation property
        //public User Users { get; set; }
    }
}
