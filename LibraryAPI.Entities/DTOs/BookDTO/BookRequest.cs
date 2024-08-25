namespace LibraryAPI.Entities.DTOs.BookDTO
{
    public class BookRequest
    {
        public string ISBN { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int PageCount { get; set; }

        public short PublishingYear { get; set; }

        public string? Description { get; set; }

        public int? PrintCount { get; set; }

        public short CopyCount { get; set; }

        public long? PublisherId { get; set; }

        public int? LocationId { get; set; }

        public List<long> AuthorIds { get; set; } = new List<long>();
        public List<short> LanguageIds { get; set; } = new List<short>();
        public List<short> SubCategoryIds { get; set; } = new List<short>();
    }
}
