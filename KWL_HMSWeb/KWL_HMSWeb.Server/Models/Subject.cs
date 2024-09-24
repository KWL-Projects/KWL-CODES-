using System.ComponentModel.DataAnnotations;

namespace KWL_HMSWeb.Server.Models
{
    public class Subject
    {
        [Key]
        public int subject_id { get; set; }
        public string subject_name { get; set; }
        public string subject_description { get; set; }
    }
}
