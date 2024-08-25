namespace LibraryAPI.Entities.DTOs.BookCopyDTO
{
    public class BookCopyResponse
    {
        public long BookId { get; set; }

        public long BookCopyId { get; set; }

        public int? LocationId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? BookCopyStatus { get; set; }
    }
}
