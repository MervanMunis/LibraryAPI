using AutoMapper;
using LibraryAPI.Entities.DTOs.LoanDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class LoanManager : ILoanService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IPenaltyService _penaltyService;

        public LoanManager(IRepositoryManager repositoryManager, IMapper mapper, IPenaltyService penaltyService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _penaltyService = penaltyService;
        }

        public async Task<IEnumerable<LoanResponse>> GetLoansByEmployeeIdAsync(string employeeId, bool trackChanges)
        {
            var loans = await _repositoryManager.LoanRepository
                .FindByCondition(l => l.EmployeeId == employeeId, trackChanges)
                .Include(l => l.Member)
                    .ThenInclude(m => m!.ApplicationUser)
                .Include(l => l.Employee)
                    .ThenInclude(e => e!.ApplicationUser)
                .Include(l => l.BookCopy)
                    .ThenInclude(bc => bc!.Book)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LoanResponse>>(loans);
        }

        public async Task<IEnumerable<LoanResponse>> GetLoansByMemberIdAsync(string memberId, bool trackChanges)
        {
            var loans = await _repositoryManager.LoanRepository
                .FindByCondition(l => l.MemberId == memberId, trackChanges)
                .Include(l => l.Member)
                    .ThenInclude(m => m!.ApplicationUser)
                .Include(l => l.Employee)
                    .ThenInclude(e => e!.ApplicationUser)
                .Include(l => l.BookCopy)
                    .ThenInclude(bc => bc!.Book)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LoanResponse>>(loans);
        }

        public async Task<LoanResponse> GetLoanByIdAsync(int id, bool trackChanges)
        {
            var loan = await _repositoryManager.LoanRepository
                .FindByCondition(l => l.LoanId == id, trackChanges)
                .Include(l => l.Member)
                    .ThenInclude(m => m!.ApplicationUser)
                .Include(l => l.Employee)
                    .ThenInclude(e => e!.ApplicationUser)
                .Include(l => l.BookCopy)
                    .ThenInclude(bc => bc!.Book)
                .FirstOrDefaultAsync();

            if (loan == null)
                throw new KeyNotFoundException("Loan not found");

            return _mapper.Map<LoanResponse>(loan);
        }

        public async Task<IEnumerable<LoanTransactionResponse>> GetLoanTransactionByLoanIdAsync(int loanId, bool trackChanges)
        {
            var transactions = await _repositoryManager.LoanTransactionRepository
                .FindByCondition(lt => lt.LoanId == loanId, trackChanges)
                .Include(lt => lt.Employee)
                    .ThenInclude(e => e!.ApplicationUser)
                .ToListAsync();

            if (!transactions.Any())
                throw new KeyNotFoundException("No loan transactions found for the given loan ID");

            return _mapper.Map<IEnumerable<LoanTransactionResponse>>(transactions);
        }

        public async Task AddLoanAsync(LoanRequest loanRequest)
        {
            var bookCopy = await _repositoryManager.BookCopyRepository
                .FindByCondition(bc => bc.BookCopyId == loanRequest.BookCopyId && bc.BookCopyStatus == Status.Active.ToString(), false)
                .FirstOrDefaultAsync();

            if (bookCopy == null)
                throw new InvalidOperationException("Book copy not available for loan");

            var member = await _repositoryManager.MemberRepository
                .FindByCondition(m => m.ApplicationUser!.IdNumber == loanRequest.MemberIdNumber, false)
                .FirstOrDefaultAsync();

            if (member == null)
                throw new KeyNotFoundException("Member with the given IDNumber does not exist");

            var employee = await _repositoryManager.EmployeeRepository
                .FindByCondition(e => e.EmployeeId == loanRequest.EmployeeId, false)
                .FirstOrDefaultAsync();

            if (employee == null)
                throw new KeyNotFoundException("Employee does not exist");

            var newLoan = new Loan
            {
                CountDay = loanRequest.HowManyDays,
                MemberId = member.MemberId,
                EmployeeId = loanRequest.EmployeeId,
                BookCopyId = loanRequest.BookCopyId,
                DueDate = DateTime.Now.AddDays(loanRequest.HowManyDays)
            };

            await _repositoryManager.LoanRepository.CreateAsync(newLoan);

            bookCopy.BookCopyStatus = Status.Borrowed.ToString();
            _repositoryManager.BookCopyRepository.Update(bookCopy);

            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateLoanAsync(LoanUpdateRequest loanUpdateRequest)
        {
            var loan = await _repositoryManager.LoanRepository.GetLoanByIdAsync(loanUpdateRequest.LoanId, true);
            if (loan == null)
                throw new KeyNotFoundException("Loan not found");

            if (loanUpdateRequest.LoanStatus == Status.Returned.ToString() && loan.LoanStatus != Status.Borrowed.ToString())
                throw new InvalidOperationException("The loan cannot be marked as returned if the book is not currently borrowed");

            if (loanUpdateRequest.LoanStatus != null && loan.LoanStatus != loanUpdateRequest.LoanStatus)
            {
                loan.LoanStatus = loanUpdateRequest.LoanStatus;

                var loanTransaction = new LoanTransaction
                {
                    LoanId = loanUpdateRequest.LoanId,
                    EmployeeId = loanUpdateRequest.EmployeeId,
                    LoanStatus = loanUpdateRequest.LoanStatus,
                };

                await _repositoryManager.LoanTransactionRepository.CreateAsync(loanTransaction);
            }

            _repositoryManager.LoanRepository.Update(loan);

            await _repositoryManager.SaveAsync();
        }

        public async Task ReturnBookAsync(int id)
        {
            var loan = await _repositoryManager.LoanRepository
                .FindByCondition(l => l.LoanId == id, true)
                .Include(l => l.BookCopy)
                .FirstOrDefaultAsync();

            if (loan == null)
                throw new KeyNotFoundException("Loan not found");

            if (loan.LoanStatus != Status.Borrowed.ToString())
                throw new InvalidOperationException("Book is not borrowed");

            loan.ReturnDate = DateTime.Now;
            loan.LoanStatus = Status.Returned.ToString();

            var loanTransaction = new LoanTransaction
            {
                LoanId = loan.LoanId,
                EmployeeId = loan.EmployeeId,
                LoanStatus = loan.LoanStatus,
            };

            await _repositoryManager.LoanTransactionRepository.CreateAsync(loanTransaction);

            loan.BookCopy!.BookCopyStatus = Status.Active.ToString();
            _repositoryManager.BookCopyRepository.Update(loan.BookCopy);

            var loanReturnRequest = new LoanReturnRequest
            {
                MemberId = loan.MemberId,
                ReturnDate = DateTime.Now,
                DueDate = loan.DueDate
            };

            await _penaltyService.CalculatePenaltiesAsync(loanReturnRequest);

            await _repositoryManager.SaveAsync();
        }
    }
}
