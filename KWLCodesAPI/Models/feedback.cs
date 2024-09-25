using System.ComponentModel.DataAnnotations;

namespace KWLCodesAPI.Models
{
    public class Feedback
    {
        [Key]
        public int feedback_id { get; set; }
        public int submission_id { get; set; }
        public int user_id { get; set; }
        public string feedback { get; set; }
        public int mark_received { get; set; }
    }
}
