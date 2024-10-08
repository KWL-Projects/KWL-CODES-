﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KWL_HMSWeb.Server.Models
{
    public class Assignment
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int assignment_id { get; set; } // Primary key

        [ForeignKey("Subject")] // Foreign key to Subject table
        public int subject_id { get; set; } // Foreign key

        [Required]
        [MaxLength(100)]
        public string assignment_name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string assignment_description { get; set; } = string.Empty;

        public DateTime due_date { get; set; }

        // Foreign key relationship
        public Subject Subject { get; set; } // Navigation property for foreign key

        // Navigation property
        //public ICollection<Submission> Submission { get; set; } // Assignment can have multiple Submissions
    }
}

