using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Peggy.Models;

namespace Peggy.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Patronage> Patronages { get; set; }
        public DbSet<PatronagePayment> PatronagePayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=peggy.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.PatronUser)
                .WithMany()
                .HasForeignKey(p => p.PatronUserId);

            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.Project)
                .WithMany(p => p.Patronages)
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<PatronagePayment>()
                .HasOne(p => p.Patronage)
                .WithMany()
                .HasForeignKey(p => p.PatronageId);
        }
    }
} 