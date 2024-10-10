using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string admin_role { get; set; } = string.Empty;

        // Navigation property for the User associated with this admin
        [ForeignKey("user_id")]
        public User User { get; set; } = null!;
    }
}
