using System.ComponentModel.DataAnnotations;

namespace KWLCodesAPI.Models
{
    public class Lecturer
    {
        [Key]
        public int user_id { get; set; }
        public int subject_id { get; set; }
        public int office_number { get; set; }
    }
}
