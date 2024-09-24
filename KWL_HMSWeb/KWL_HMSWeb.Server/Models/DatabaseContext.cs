using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;

namespace KWL_HMSWeb.Server.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Assignment> Assignment { get; set; } = null!;
        public DbSet<Feedback> Feedback { get; set; } = null!;
        public DbSet<Lecturer> Lecturer { get; set; } = null!;
        public DbSet<Student> Student { get; set; } = null!;
        public DbSet<Admin> Admin { get; set; } = null!;
        public DbSet<Enrollment> Enrollment { get; set; } = null!;
        public DbSet<Subject> Subject { get; set; } = null!;
        public DbSet<Submission> Submission { get; set; } = null!;
    }
}
