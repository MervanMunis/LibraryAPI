using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookLanguageRepository : RepositoryBase<BookLanguage>, IBookLanguageRepository
    {
        public BookLanguageRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<BookLanguage>> GetAllBookLanguagesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<BookLanguage> GetBookLanguageByIdAsync(long bookId, short languageId, bool trackChanges) =>
            await FindByCondition(bl => bl.BooksId == bookId && bl.LanguagesId == languageId, trackChanges).SingleOrDefaultAsync();
    }
}
