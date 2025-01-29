namespace TaskManagerAPI.Models
{
    public class TaskUpdateRequest
    {
        public int Id { get; set; } // Task ID to update

        public string? Title { get; set; } // Optional new title

        public string? Description { get; set; } // Optional new description

        public bool? Status { get; set; } // Optional new status
    }
}
