﻿namespace KWLCodesAPI.Models
{
    public class enrollment
    {
        public int user_id {  get; set; }
        public int subject_id { get; set; }
        public string enrollment_description { get; set; }
        public DateOnly enrollment_startDate { get; set; }
        public DateOnly enrollment_endDate { get; set; }
    }
}
