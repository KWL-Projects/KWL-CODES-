using System.ComponentModel.DataAnnotations; 
namespace KWLCodesAPI.Models
{
    public class Admin
    {
        [Key]
        public int user_id { get; set; }
        public string admin_Role { get; set; }
    }
}
