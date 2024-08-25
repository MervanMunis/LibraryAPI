namespace LibraryAPI.Entities.DTOs.LoanDTO
{
    public class LoanReturnRequest
    {
        public string MemberId { get; set; } = string.Empty;

        public DateTime? ReturnDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
