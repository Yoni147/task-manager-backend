using Microsoft.EntityFrameworkCore;

namespace TaskManagerAPI.Models
{
    // Represents the application's database context
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Represents the Tasks table in the database
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
