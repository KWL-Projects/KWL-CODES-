using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Lecturer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required]
        public int subject_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string office_number { get; set; } = string.Empty;

        // Navigation property for the User associated with this lecturer
        [ForeignKey("user_id")]
        public User User { get; set; } = null!;

        // Navigation property for the Subject associated with this lecturer
        [ForeignKey("subject_id")]
        public Subject Subject { get; set; } = null!;
    }
}


