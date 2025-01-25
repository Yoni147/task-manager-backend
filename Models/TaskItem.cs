
namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        private string _title;
        private string _description;


        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? "Untitled Task" : value;
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? "No Description" : value;
        }

        public bool Status { get; set; }

        // Optional: Constructor to initialize default values
        public TaskItem()
        {
            _title = "Untitled Task";
            _description = "No Description";
        }
    }

}
