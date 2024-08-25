using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IWantedBookRepository : IRepositoryBase<WantedBook>
    {
        Task<IEnumerable<WantedBook>> GetAllWantedBooksAsync(bool trackChanges);
        Task<WantedBook> GetWantedBookByIdAsync(int wantedBookId, bool trackChanges);
    }
}
