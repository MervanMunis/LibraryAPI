using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Loan> GetLoanByIdAsync(int loanId, bool trackChanges) =>
            await FindByCondition(l => l.LoanId.Equals(loanId), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Loan>> GetLoansByEmployeeIdAsync(string employeeId, 
            bool trackChanges) => 
            await FindByCondition(l => l.EmployeeId.Equals(employeeId), trackChanges)
            .ToListAsync();


        public async Task<IEnumerable<Loan>> GetLoansByMemberIdAsync(string memberId, 
            bool trackChanges) =>
            await FindByCondition(l => l.MemberId.Equals(memberId), trackChanges)
            .ToListAsync();

    }
}
