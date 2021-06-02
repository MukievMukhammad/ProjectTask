using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

namespace TaskTracker.DB
{
    public class TaskTrackerDbContext : DbContext
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        
        public TaskTrackerDbContext(){}
        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options) {}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasDefaultValue(ProjectStatus.NotStarted);
        }
    }
}