using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private static List<TaskItem> tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "משימה ראשונה", Description = "תיאור משימה", Status = false },
            new TaskItem { Id = 2, Title = "משימה שנייה", Description = "תיאור נוסף", Status = true }
        };

        // GET: api/tasks
        [HttpGet]
        public IActionResult GetAllTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pagedTasks = tasks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalItems = tasks.Count,
                Page = page,
                PageSize = pageSize,
                Tasks = pagedTasks
            });
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { Message = "המשימה לא נמצאה" });
            }
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            if (string.IsNullOrEmpty(task.Title) || task.Title.Length < 3 || task.Title.Length > 50)
            {
                return BadRequest(new { Message = "כותרת המשימה חייבת להיות בין 3 ל-50 תווים." });
            }

            task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { Message = "המשימה לא נמצאה" });
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;

            return Ok(task);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { Message = "המשימה לא נמצאה" });
            }

            tasks.Remove(task);
            return NoContent();
        }
    }
}
