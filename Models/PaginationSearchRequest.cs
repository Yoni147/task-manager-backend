
namespace TaskManagerAPI.Models
{
    public class PaginationSearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchQuery { get; set; }
    }

    
}


