using Microsoft.EntityFrameworkCore;
using Peggy.Models;

namespace Peggy.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Patronage> Patronages { get; set; }
        public DbSet<PatronagePayment> PatronagePayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Project's self-referencing relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.ChildProjects)
                .HasForeignKey(p => p.ProjectParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Patronage
            modelBuilder.Entity<Patronage>()
                .HasKey(p => p.PatronageId);

            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.User)
                .WithMany(u => u.Patronages)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patronage>()
                .HasOne(p => p.Project)
                .WithMany(p => p.Patronages)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure PatronagePayment
            modelBuilder.Entity<PatronagePayment>()
                .HasKey(p => p.PaymentId);

            modelBuilder.Entity<PatronagePayment>()
                .HasOne(p => p.Patronage)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PatronageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 