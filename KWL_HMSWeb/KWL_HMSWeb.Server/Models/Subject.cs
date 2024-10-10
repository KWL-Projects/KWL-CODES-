using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int subject_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string subject_name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? subject_description { get; set; }

        public Lecturer Lecturer { get; set; } = null!;
        public Enrollment Enrollment { get; set; } = null!;
        public Assignment Assignment { get; set; } = null!;
    }
}

