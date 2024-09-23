namespace KWLCodesAPI.Models
{
    public class assignment
    {
        public int assignment_id {  get; set; }
        public int subject_id { get; set; }
        public int user_id { get; set; }
        public string assignment_name { get; set; }
        public string assignment_description { get; set; }
        public DateTime due_date { get; set; }
    }
}
