using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Lecturer
    {
        /*[Key, ForeignKey("User")]
        public int user_id { get; set; }

        public int subject_id { get; set; }
        public string office_number { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Subject Subject { get; set; }*/

        [Key]
        public int user_id {  get; set; }
        public int subject_id { get; set; }
        public string office_number { get; set; }
    }
}

