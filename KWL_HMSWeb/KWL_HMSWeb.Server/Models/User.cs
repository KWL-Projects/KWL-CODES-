using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class User
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; } // Primary key

        [ForeignKey("Login")] // Foreign key to Login table
        public int login_id { get; set; } // Foreign key

        [Required]
        [MaxLength(50)]
        public string user_firstname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string user_surname { get; set; } = string.Empty;

        [Required]
        public string user_type { get; set; } = string.Empty;

        // Foreign key relationship
        public Login Login { get; set; } // Navigation property for foreign key

        // Navigation property
        //public ICollection<Subject> Subject { get; set; } // User can have multiple Subjects

        //public ICollection<Submission> Submission { get; set; } // User can have multiple Submissions
    }
}


