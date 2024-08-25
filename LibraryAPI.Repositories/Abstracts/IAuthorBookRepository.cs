using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IAuthorBookRepository : IRepositoryBase<AuthorBook>
    {
        Task<IEnumerable<AuthorBook>> GetAllAuthorBooksAsync(bool trackChanges);
        Task<AuthorBook> GetAuthorBookByIdAsync(long authorId, long bookId, bool trackChanges);
    }
}
