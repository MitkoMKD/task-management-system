using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>().ToTable("Tasks");
            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = "hashedpassword" } 
        );
        }
    }
}
