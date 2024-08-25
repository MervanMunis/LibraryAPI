namespace LibraryAPI.Entities.DTOs.BookDTO
{
    public class CategoryBookResponse
    {

        public long BookId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public List<string>? SubCategoryNames { get; set; }
    }
}
