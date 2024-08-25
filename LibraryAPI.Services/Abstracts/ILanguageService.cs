using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LanguageDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageResponse>> GetAllLanguagesAsync(bool trackChanges);
        Task<LanguageResponse> GetLanguageByIdAsync(short id, bool trackChanges);
        Task AddLanguageAsync(LanguageRequest languageRequest);
        Task UpdateLanguageAsync(short id, LanguageRequest languageRequest);
        Task SetLanguageStatusAsync(short id, string status);
        Task<IEnumerable<BookResponse>> GetBooksByLanguageIdAsync(short languageId, bool trackChanges);
    }
}
