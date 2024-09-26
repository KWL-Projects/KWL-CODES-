using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int assignment_id { get; set; }

        [Required]
        public int subject_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string assignment_name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? assignment_description { get; set; }

        [DataType(DataType.Date)]
        public DateTime due_date { get; set; }

        // Navigation properties
        [ForeignKey("user_id")]
        public User User { get; set; } = null!;

        [ForeignKey("subject_id")]
        public Subject Subject { get; set; } = null!;
        public Submission Submission {  get; set; } = null!;
    }
}

