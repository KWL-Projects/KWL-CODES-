using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required]
        public int login_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string user_firstname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string user_surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string user_type { get; set; } = string.Empty;

        // Navigation property for the related Login
        [ForeignKey("login_id")]
        public Login Login { get; set; } = null!;

        public Admin Admin { get; set; } = null!;

        public Student Student { get; set; } = null!;

        public Lecturer Lecturer {  get; set; } = null!;
    }
}


