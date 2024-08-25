namespace LibraryAPI.Entities.DTOs.LoanDTO
{
    public class LoanTransactionResponse
    {
        public int LoanId { get; set; }

        public string EmployeeId { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public string? LoanStatus { get; set; }

        public DateTime LoanUpdate { get; set; }
    }
}
