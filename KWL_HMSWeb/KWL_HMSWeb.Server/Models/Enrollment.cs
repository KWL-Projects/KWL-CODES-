using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Enrollment
    {
        [Key]
        public int enrollment_id { get; set; }

        public int user_id { get; set; }
        public int subject_id { get; set; }

        public string enrollment_description { get; set; }
        public DateTime enrollment_startDate { get; set; }
        public DateTime enrollment_endDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Subject Subject { get; set; }
    }
}

