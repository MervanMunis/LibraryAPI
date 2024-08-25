namespace LibraryAPI.Entities.DTOs.BookDTO
{
    public class BookResponse
    {
        public long BookId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int PageCount { get; set; }

        public short PublishingYear { get; set; }

        public string? Description { get; set; }

        public int? PrintCount { get; set; }

        public short CopyCount { get; set; }

        public string? BookStatus { get; set; }

        public string? PublisherName { get; set; }

        public float? Rating { get; set; }

        public List<string>? SubCategoryNames { get; set; }

        public string? CategoryName { get; set; }

        public List<string>? LanguageNames { get; set; }

        public List<string>? AuthorNames { get; set; }
    }
}
