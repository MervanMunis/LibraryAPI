using LibraryAPI.Entities.DTOs.LoanDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanResponse>> GetLoansByEmployeeIdAsync(string employeeId, bool trackChanges);
        Task<IEnumerable<LoanResponse>> GetLoansByMemberIdAsync(string memberId, bool trackChanges);
        Task<LoanResponse> GetLoanByIdAsync(int id, bool trackChanges);
        Task<IEnumerable<LoanTransactionResponse>> GetLoanTransactionByLoanIdAsync(int loanId, bool trackChanges);
        Task AddLoanAsync(LoanRequest loanRequest);
        Task UpdateLoanAsync(LoanUpdateRequest loanUpdateRequest);
        Task ReturnBookAsync(int id);
    }
}
