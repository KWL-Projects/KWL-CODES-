using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace KWL_HMSWeb.Server.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [ForeignKey("Login")]
        public int login_id { get; set; }

        public string user_firstname { get; set; }
        public string user_surname { get; set; }
        public string user_type { get; set; }

        // Navigation property for related entities
        public virtual Login Login { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Student Student { get; set; }
        public virtual Lecturer Lecturer { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}

