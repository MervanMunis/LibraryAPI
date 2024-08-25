using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        public BookCopyRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<BookCopy>> GetAllActiveBookCopiesAsync(string active, bool trackChanges)
            => await FindByCondition(a => a.BookCopyStatus.Equals(active), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();

        public async Task<IEnumerable<BookCopy>> GetAllBorrowedBookCopiesAsync(string borrowed, bool trackChanges)
            => await FindByCondition(a => a.BookCopyStatus.Equals(borrowed), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<BookCopy>> GetAllInActiveBookCopiesAsync(string inActive, bool trackChanges)
            => await FindByCondition(a => a.BookCopyStatus.Equals(inActive), trackChanges)
            .ToListAsync();
    }
}
