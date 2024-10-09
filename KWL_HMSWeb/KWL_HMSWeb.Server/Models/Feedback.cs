using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Add this for System.Text.Json.JsonIgnore

namespace KWL_HMSWeb.Server.Models
{
    public class Feedback
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int feedback_id { get; set; } // Primary key

        [ForeignKey("Submission")] // Foreign key to Submission table
        public int submission_id { get; set; } // Foreign key

        [Required]
        public string feedback { get; set; } = string.Empty;

        public int mark_received { get; set; }

        // Foreign key relationship
        [JsonIgnore]
        public Submission? Submission { get; set; } // Navigation property for foreign key
    }
}

