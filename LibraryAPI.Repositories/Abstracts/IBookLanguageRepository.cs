using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IBookLanguageRepository : IRepositoryBase<BookLanguage>
    {
        Task<IEnumerable<BookLanguage>> GetAllBookLanguagesAsync(bool trackChanges);
        Task<BookLanguage> GetBookLanguageByIdAsync(long bookId, short languageId, bool trackChanges);
    }
}
