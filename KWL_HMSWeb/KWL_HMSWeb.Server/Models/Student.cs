using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        public int user_id { get; set; }

        public string student_number { get; set; }

        // Navigation property
        public virtual User User { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}

