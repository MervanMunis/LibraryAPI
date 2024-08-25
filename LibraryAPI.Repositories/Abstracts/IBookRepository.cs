using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
        Task<IEnumerable<Book>> GetAllActiveBooksAsync(string active, bool trackChanges);
        Task<IEnumerable<Book>> GetAllInActiveBooksAsync(string inActive, bool trackChanges);
        Task<IEnumerable<Book>> GetAllBannedBooksAsync(string banned, bool trackChanges);
        Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(long publisherId, bool trackChanges);
        Task<IEnumerable<Book>> GetBooksBySubCategoryIdAsync(short subCategoryId, bool trackChanges);
        Task<Book> GetBookByIdAsync(long bookId, bool trackChanges);
    }
}
