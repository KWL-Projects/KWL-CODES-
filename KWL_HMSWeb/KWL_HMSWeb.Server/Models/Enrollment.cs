using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Enrollment
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int subject_id { get; set; }

        [MaxLength(50)]
        public string? enrollment_description { get; set; }

        [DataType(DataType.Date)]
        public DateTime enrollment_startDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime enrollment_endDate { get; set; }

        // Navigation properties
        [ForeignKey("user_id")]
        public User User { get; set; } = null!;

        [ForeignKey("subject_id")]
        public Subject Subject { get; set; } = null!;
    }
}


