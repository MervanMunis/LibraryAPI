using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ILoanRepository : IRepositoryBase<Loan>
    {
        Task<IEnumerable<Loan>> GetLoansByEmployeeIdAsync(string employeeId, bool trackChanges);
        Task<IEnumerable<Loan>> GetLoansByMemberIdAsync(string memberId, bool trackChanges);
        Task<Loan> GetLoanByIdAsync(int loanId, bool trackChanges);
    }
}
