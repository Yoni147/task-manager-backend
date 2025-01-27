using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public static class TaskSeeder
    {
        // Seeds mock tasks into the database
        public static void SeedTasks(AppDbContext context)
        {
            // Check if the database already has tasks
            if (!context.Tasks.Any())
            {
                var tasks = new List<TaskItem>();

                // Generate 150 mock tasks
                for (int i = 1; i <= 150; i++)
                {
                    tasks.Add(new TaskItem
                    {
                        Title = $"Task {i}",
                        Description = $"Description for task {i}",
                        Status = i % 2 == 0 // כל משימה זוגית תהיה true
                    });
                }

                // Add tasks to the database
                context.Tasks.AddRange(tasks);
                context.SaveChanges(); // Save changes to the in-memory database
            }
        }
    }
}
