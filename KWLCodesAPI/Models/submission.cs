﻿using System.ComponentModel.DataAnnotations;

namespace KWLCodesAPI.Models
{
    public class Submission
    {
        [Key]
        public int submission_id { get; set; }
        public int assignment_id { get; set; }
        public DateTime submission_date { get; set; }
        public string submission_description { get; set; }
    }
}
