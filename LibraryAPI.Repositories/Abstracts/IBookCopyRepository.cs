using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IBookCopyRepository : IRepositoryBase<BookCopy>
    {
        Task<IEnumerable<BookCopy>> GetAllBookCopiesAsync(bool trackChanges);
        Task<IEnumerable<BookCopy>> GetAllActiveBookCopiesAsync(string active, bool trackChanges);
        Task<IEnumerable<BookCopy>> GetAllInActiveBookCopiesAsync(string inActive, bool trackChanges);
        Task<IEnumerable<BookCopy>> GetAllBorrowedBookCopiesAsync(string borrowed, bool trackChanges);
    }
}