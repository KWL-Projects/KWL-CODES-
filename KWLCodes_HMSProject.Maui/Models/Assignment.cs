using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class Assignment
    {
        public int assignment_id { get; set; } // Primary key
        public int subject_id { get; set; } // Foreign key
        public string assignment_name { get; set; } = string.Empty;
        public string assignment_description { get; set; } = string.Empty;
        public DateTime due_date { get; set; }
    }
}

