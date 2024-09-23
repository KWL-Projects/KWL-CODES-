using System.ComponentModel.DataAnnotations;

namespace KWLCodesAPI.Models
{
    public class Student
    {
        [Key]
        public int user_id { get; set; }
        public int student_number { get; set; }
    }
}
