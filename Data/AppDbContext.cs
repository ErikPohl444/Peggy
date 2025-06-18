using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Peggy.Models;

namespace Peggy.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCollection> ProjectCollections { get; set; }
        public DbSet<Patronage> Patronages { get; set; }
        public DbSet<PatronagePayment> PatronagePayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=peggy.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ProjectCollection relationships
            modelBuilder.Entity<ProjectCollection>()
                .HasOne(pc => pc.Owner)
                .WithMany()
                .HasForeignKey(pc => pc.OwnerUserId);

            modelBuilder.Entity<ProjectCollection>()
                .HasMany(pc => pc.Projects)
                .WithOne(p => p.Collection)
                .HasForeignKey(p => p.CollectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Project relationships
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerUserId);

            // Configure Patronage relationships
            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.PatronUser)
                .WithMany()
                .HasForeignKey(p => p.PatronUserId);

            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.Project)
                .WithMany(p => p.Patronages)
                .HasForeignKey(p => p.ProjectId);

            // Configure PatronagePayment relationships
            modelBuilder.Entity<PatronagePayment>()
                .HasOne(p => p.Patronage)
                .WithMany()
                .HasForeignKey(p => p.PatronageId);
        }
    }
} 