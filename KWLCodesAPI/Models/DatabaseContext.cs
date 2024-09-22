using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;

namespace KWLCodesAPI.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<login> Login { get; set; }
        public DbSet<user> User { get; set; }
        public DbSet<assignment> Assignment { get; set; }
        public DbSet<feedback> Feedback { get; set; }
        public DbSet<lecturer> Lecturer { get; set; }
        public DbSet<student> Student { get; set; }
        public DbSet<admin> Admin { get; set; }
        public DbSet<enrollment> Enrollment { get; set; }
        public DbSet<subject> Subject { get; set; }
        public DbSet<submission> Submission { get; set; }
    }
}
