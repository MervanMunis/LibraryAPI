using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ILanguageRepository : IRepositoryBase<Language>
    {
        Task<IEnumerable<Language>> GetAllLanguagesAsync(bool trackChanges);
        Task<Language> GetLanguageByIdAsync(short languageId, bool trackChanges);
    }
}
