namespace KWLCodes_HMSProject.Maui.Models
{
    public class Assignment
    {
        public int assignment_id { get; set; } // Primary key
        public int subject_id { get; set; } // Foreign key
        public string assignment_name { get; set; } = string.Empty; // Changed from Title to assignment_name
        public string assignment_description { get; set; } = string.Empty; // Changed from Description to assignment_description
        public DateTime due_date { get; set; }
    }
}
