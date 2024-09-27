using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;

namespace KWL_HMSWeb.Server.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Assignment> Assignment { get; set; } = null!;
        public DbSet<Feedback> Feedback { get; set; } = null!;
        public DbSet<Lecturer> Lecturer { get; set; } = null!;
        public DbSet<Student> Student { get; set; } = null!;
        public DbSet<Admin> Admin { get; set; } = null!;
        public DbSet<Enrollment> Enrollment { get; set; } = null!;
        public DbSet<Subject> Subject { get; set; } = null!;
        public DbSet<Submission> Submission { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships if needed

             modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.user_id, e.subject_id }); // Configure composite key

            /*modelBuilder.Entity<Login>()
                .HasKey(l => l.login_id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.user_id);

            modelBuilder.Entity<Admin>()
                .HasKey(a => a.user_id);

            modelBuilder.Entity<Student>()
                .HasKey(s => s.user_id);

            modelBuilder.Entity<Lecturer>()
                .HasKey(l => l.user_id);

            modelBuilder.Entity<Subject>()
                .HasKey(s => s.subject_id);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.user_id, e.subject_id });

            modelBuilder.Entity<Assignment>()
                .HasKey(a => new { a.subject_id, a.user_id });

            modelBuilder.Entity<Submission>()
                .HasKey(s => s.submission_id);

            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.feedback_id);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne<Login>()
                .WithMany()
                .HasForeignKey(u => u.login_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lecturer>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne<Subject>()
                .WithMany()
                .HasForeignKey(e => e.subject_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne<Subject>()
                .WithMany()
                .HasForeignKey(a => a.subject_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Submission>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne<Submission>()
                .WithMany()
                .HasForeignKey(f => f.submission_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.user_id)
                .OnDelete(DeleteBehavior.Cascade);*/

        }
    }
}
