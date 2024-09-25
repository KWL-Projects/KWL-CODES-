using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KWL_HMSWeb.Server.Models
{
    public class Subject
    {
        [Key]
        public int subject_id { get; set; }

        public string subject_name { get; set; }
        public string subject_description { get; set; }

        // Navigation properties
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
