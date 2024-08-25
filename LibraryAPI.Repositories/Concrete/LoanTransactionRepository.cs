using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class LoanTransactionRepository : RepositoryBase<LoanTransaction>, ILoanTransactionRepository
    {
        public LoanTransactionRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransactionByLoanId(int loanId, bool trackChanges)
            => await FindByCondition(t => t.Loan!.Equals(loanId), trackChanges).ToListAsync();
    }
}
