using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync(bool trackChanges);
        Task<Author> GetAuthorByIdAsync(long authorId, bool trackChanges);
        Task<IEnumerable<Author>> GetAllActiveAuthorsAsync(string active, bool trackChanges);
        Task<IEnumerable<Author>> GetAllInActiveAuthorsAsync(string inActive, bool trackChanges);
        Task<IEnumerable<Author>> GetAllBannedAuthorsAsync(string banned, bool trackChanges);
    }
}
