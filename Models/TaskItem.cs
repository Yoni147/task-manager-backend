using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {

        private string _title = "Untitled Task";
        private string _description = "Untitled Task";

        public int Id { get; set; }

        [Required(ErrorMessage = "כותרת המשימה היא שדה חובה.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "כותרת המשימה חייבת להיות בין 3 ל-50 תווים.")]
        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? "Untitled Task" : value;
        }

        [StringLength(200, ErrorMessage = "תיאור המשימה יכול להכיל עד 200 תווים.")]
        public string Description 
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ?  "No Description" : value; 
        } 

        [Required(ErrorMessage = "סטטוס הוא שדה חובה.")]
        public bool Status { get; set; }
    }
}
