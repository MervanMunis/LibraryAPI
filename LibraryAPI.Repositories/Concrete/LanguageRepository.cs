using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        public LanguageRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync(bool trackChanges) 
            => await FindAll(trackChanges).Include(n => n.Nationality).ToListAsync();

        public async Task<Language> GetLanguageByIdAsync(short languageId, bool trackChanges) =>
            await FindByCondition(l => l.LanguageId.Equals(languageId), trackChanges).SingleOrDefaultAsync();
    }
}
