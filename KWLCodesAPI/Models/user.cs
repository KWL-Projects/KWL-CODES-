namespace KWLCodesAPI.Models
{
    public class User
    {
        public int user_id { get; set; }
        public int login_id {  get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public char user_type { get; set; }
    }
}
