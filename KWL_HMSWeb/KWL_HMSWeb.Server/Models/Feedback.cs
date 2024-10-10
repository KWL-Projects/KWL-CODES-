using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int feedback_id { get; set; }

        [Required]
        public int submission_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [MaxLength(50)]
        public string feedback { get; set; } = string.Empty;

        public int mark_received { get; set; }

        // Navigation properties
        [ForeignKey("submission_id")]
        public Submission Submission { get; set; } = null!;

        [ForeignKey("user_id")]
        public User User { get; set; } = null!;
    }
}

