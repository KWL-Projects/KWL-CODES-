using System.ComponentModel.DataAnnotations;

namespace KWL_HMSWeb.Server.Models
{
    public class Lecturer
    {
        [Key]
        public int user_id { get; set; }
        public int subject_id { get; set; }
        public int office_number { get; set; }
    }
}
