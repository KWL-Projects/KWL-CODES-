using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int submission_id { get; set; }

        [Required]
        public int assignment_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [DataType(DataType.Date)]
        public DateTime submission_date { get; set; }

        [MaxLength(50)]
        public string? submission_description { get; set; }

        [MaxLength(50)]
        public string? video_path { get; set; }

        // Navigation property
        [ForeignKey("assignment_id")]
        public Assignment Assignment { get; set; } = null!;

        [ForeignKey("user_id")]
        public User User { get; set; } = null!;
        public Feedback Feedback {  get; set; } = null!;
    }
}

