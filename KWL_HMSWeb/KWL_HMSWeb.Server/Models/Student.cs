using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required]
        [MaxLength(20)]
        public string student_number { get; set; } = string.Empty;

        // Navigation property for the User associated with this student
        [ForeignKey("user_id")]
        public User User { get; set; } = null!;
    }
}


