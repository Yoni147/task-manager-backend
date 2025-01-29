using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using System.Linq;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor to inject AppDbContext
        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/tasks - Retrieve tasks with pagination and search
        [HttpPost("")]
        public IActionResult GetAllTasksWithPaginationAndSearch([FromBody] PaginationSearchRequest request)
        {
            if (request.Page < 1 || request.PageSize < 1)
            {
                return BadRequest(new { error = "Invalid request parameters. Page and PageSize must be greater than 0." });
            }

            var filteredTasks = string.IsNullOrWhiteSpace(request.SearchQuery)
                ? _context.Tasks.ToList()
                : _context.Tasks
                    .Where(t =>
                        t.Title.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                        t.Description.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var pagedTasks = filteredTasks
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return Ok(new
            {
                tasks = pagedTasks,
                total = filteredTasks.Count
            });
        }

        // POST: api/tasks/details - Retrieve task by ID (moved from URL to body)
        [HttpPost("details")]
        public IActionResult GetTaskById([FromBody] TaskIdRequest request)
        {
            if (request == null || request.Id <= 0)
            {
                return BadRequest(new { error = "Invalid task ID." });
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == request.Id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found." });
            }

            return Ok(task);
        }

        // POST: api/tasks/create - Create a new task
        [HttpPost("create")]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(task.Title) || task.Title.Length < 3 || task.Title.Length > 50)
            {
                return BadRequest(new { error = "Task title must be between 3 and 50 characters." });
            }

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT: api/tasks/update - Update task (moved ID to body)
        [HttpPut("update")]
        public IActionResult UpdateTask([FromBody] TaskUpdateRequest request)
        {
            if (request == null || request.Id <= 0)
            {
                return BadRequest(new { error = "Invalid task ID." });
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == request.Id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found." });
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                task.Title = request.Title;
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                task.Description = request.Description;
            }
            if (request.Status.HasValue)
            {
                task.Status = request.Status.Value;
            }

            _context.SaveChanges();

            return Ok(task);
        }

        // DELETE: api/tasks/delete - Delete task (moved ID to body)
        [HttpPost("delete")]
        public IActionResult DeleteTask([FromBody] TaskIdRequest request)
        {
            if (request == null || request.Id <= 0)
            {
                return BadRequest(new { error = "Invalid task ID." });
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == request.Id);
            if (task == null)
            {
                return NotFound(new { error = "Task not found." });
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
