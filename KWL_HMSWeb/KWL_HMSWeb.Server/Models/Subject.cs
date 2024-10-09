using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Add this for System.Text.Json.JsonIgnore

namespace KWL_HMSWeb.Server.Models
{
    public class Subject
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int subject_id { get; set; } // Primary key

        [ForeignKey("User")] // Foreign key to User table
        public int user_id { get; set; } // Foreign key

        [Required]
        [MaxLength(100)]
        public string subject_name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string subject_description { get; set; } = string.Empty;

        // Foreign key relationship
        [JsonIgnore]
        public User? User { get; set; } // Navigation property for foreign key
    }
}

