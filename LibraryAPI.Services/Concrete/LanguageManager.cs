using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LanguageDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class LanguageManager : ILanguageService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public LanguageManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LanguageResponse>> GetAllLanguagesAsync(bool trackChanges)
        {
            var languages = await _repositoryManager.LanguageRepository.GetAllLanguagesAsync(trackChanges);
            return _mapper.Map<IEnumerable<LanguageResponse>>(languages);
        }

        public async Task<LanguageResponse> GetLanguageByIdAsync(short id, bool trackChanges)
        {
            var language = await _repositoryManager.LanguageRepository.GetLanguageByIdAsync(id, trackChanges);
            if (language == null || language.LanguageStatus != Status.Active.ToString())
                throw new KeyNotFoundException("Language not found or not active");

            return _mapper.Map<LanguageResponse>(language);
        }

        public async Task AddLanguageAsync(LanguageRequest languageRequest)
        {
            var existingLanguage = await _repositoryManager.LanguageRepository
                .FindByCondition(l => l.Name == languageRequest.Name, false)
                .FirstOrDefaultAsync();

            if (existingLanguage != null)
                throw new InvalidOperationException("Language with the specified name already exists!");

            var newLanguage = _mapper.Map<Language>(languageRequest);

            await _repositoryManager.LanguageRepository.CreateAsync(newLanguage);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateLanguageAsync(short id, LanguageRequest languageRequest)
        {
            var language = await _repositoryManager.LanguageRepository.GetLanguageByIdAsync(id, true);
            if (language == null)
                throw new KeyNotFoundException("Language not found");

            var existingLanguage = await _repositoryManager.LanguageRepository
                .FindByCondition(l => l.Name == languageRequest.Name && l.LanguageId != id, false)
                .FirstOrDefaultAsync();

            if (existingLanguage != null)
                throw new InvalidOperationException("Language with the specified name already exists!");

            _mapper.Map(languageRequest, language);
            _repositoryManager.LanguageRepository.Update(language);
            await _repositoryManager.SaveAsync();
        }

        public async Task SetLanguageStatusAsync(short id, string status)
        {
            var language = await _repositoryManager.LanguageRepository.GetLanguageByIdAsync(id, true);
            if (language == null)
                throw new KeyNotFoundException("Language not found");

            language.LanguageStatus = status;
            _repositoryManager.LanguageRepository.Update(language);

            var bookLanguages = await _repositoryManager.BookLanguageRepository
                .FindByCondition(bl => bl.LanguagesId == id, true)
                .Include(bl => bl.Book)
                .ToListAsync();

            foreach (var bookLanguage in bookLanguages)
            {
                bookLanguage.Book!.BookStatus = status;
                _repositoryManager.BookRepository.Update(bookLanguage.Book);
            }

            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByLanguageIdAsync(short languageId, bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository
                .FindByCondition(b => b.BookLanguages!.Any(bl => bl.LanguagesId == languageId), trackChanges)
                .Include(b => b.BookLanguages)!
                .ThenInclude(bl => bl.Language)
                .ToListAsync();

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this language");

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }
    }
}
