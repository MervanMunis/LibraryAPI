using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ILoanTransactionRepository : IRepositoryBase<LoanTransaction>
    {
        Task<IEnumerable<LoanTransaction>> GetLoanTransactionByLoanId(int loanId, bool trackChanges);
    }
}
