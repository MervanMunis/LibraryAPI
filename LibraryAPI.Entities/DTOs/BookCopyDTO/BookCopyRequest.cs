namespace LibraryAPI.Entities.DTOs.BookCopyDTO
{
    public class BookCopyRequest
    {
        public long BookCopyId { get; set; }

        public int? LocationId { get; set; }
    }
}
