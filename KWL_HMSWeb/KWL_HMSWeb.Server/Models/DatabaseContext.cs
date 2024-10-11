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
        public DbSet<Subject> Subject { get; set; } = null!;
        public DbSet<Assignment> Assignment { get; set; } = null!;
        public DbSet<Submission> Submission { get; set; } = null!;
        public DbSet<Feedback> Feedback { get; set; } = null!;
    }
}
