namespace LibraryAPI.Entities.DTOs.LoanDTO
{
    public class LoanResponse
    {
        public int LoanId { get; set; }

        public long BookCopyId { get; set; }

        public DateTime LoanedDate { get; set; }

        public short CountDay { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public string? LoanStatus { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public string MemberName { get; set; } = string.Empty;

        public string MemberIdNumber { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeIdNumber { get; set; } = string.Empty;
    }
}
