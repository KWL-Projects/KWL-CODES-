using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KWLCodes_HMSProject.Maui.Models
{
    public class Feedback
    {
        public int feedback_id { get; set; } // Primary key
        public int submission_id { get; set; } // Foreign key
        public string feedback { get; set; } = string.Empty;
        public decimal mark_received { get; set; }
    }
}

