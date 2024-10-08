using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Submission
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int submission_id { get; set; } // Primary key

        [ForeignKey("Assignment")] // Foreign key to Assignment table
        public int assignment_id { get; set; } // Foreign key

        [ForeignKey("User")] // Foreign key to Assignment table
        public int user_id { get; set; } // Foreign key

        public DateTime submission_date { get; set; }

        [MaxLength(500)]
        public string submission_description { get; set; } = string.Empty;

        [MaxLength(200)]
        public string video_path { get; set; } = string.Empty;

        // Foreign key relationship
        public Assignment Assignment { get; set; } // Navigation property for foreign key

        public User Users { get; set; } // Navigation property for foreign key

        // Navigation property
        public ICollection<Feedback> Feedback { get; set; } // Submission can have multiple Feedbacks
    }
}

