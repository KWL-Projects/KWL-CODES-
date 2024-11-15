﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Add this for System.Text.Json.JsonIgnore

namespace KWL_HMSWeb.Server.Models
{
    public class Submission
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int submission_id { get; set; } // Primary key

        [ForeignKey("Assignment")] // Foreign key to Assignment table
        public int assignment_id { get; set; } // Foreign key

        [ForeignKey("User")] // Foreign key to Assignment table
        public int user_id { get; set; } // Foreign key

        public DateTime submission_date { get; set; }

        [MaxLength(500)]
        public string submission_description { get; set; } = string.Empty;

        [MaxLength(200)]
        public string video_path { get; set; } = string.Empty;

        // Foreign key relationship
        [JsonIgnore]
        public Assignment? Assignment { get; set; } // Navigation property for foreign key

        [JsonIgnore]
        public User? User { get; set; } // Navigation property for foreign key
    }
}

