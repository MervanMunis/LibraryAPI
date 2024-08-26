using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(b => b.BookSubCategories)!
                .ThenInclude(bsc => bsc.SubCategory)
                .ThenInclude(sc => sc!.Category)
            .Include(b => b.BookLanguages)!
                .ThenInclude(bl => bl.Language)
            .Include(b => b.AuthorBooks)!
                .ThenInclude(ab => ab.Author)
            .Include(b => b.Publisher)
            .ToListAsync();

        public async Task<IEnumerable<Book>> GetAllActiveBooksAsync(string active, bool trackChanges)
            => await FindByCondition(a => a.BookStatus.Equals(active), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Book>> GetAllBannedBooksAsync(string banned, bool trackChanges)
            => await FindByCondition(a => a.BookStatus.Equals(banned), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Book>> GetAllInActiveBooksAsync(string inActive, bool trackChanges)
            => await FindByCondition(a => a.BookStatus.Equals(inActive), trackChanges)
            .ToListAsync();

        public async Task<Book> GetBookByIdAsync(long bookId, bool trackChanges) =>
            await FindByCondition(b => b.BookId.Equals(bookId), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(long publisherId, bool trackChanges) => 
            await FindByCondition(b => b.PublisherId == publisherId, trackChanges)
                .ToListAsync();

        public async Task<IEnumerable<Book>> GetBooksBySubCategoryIdAsync(short subCategoryId, bool trackChanges) =>
            await FindByCondition(b => b.BookSubCategories!.Any(bsc => bsc.SubCategoriesId == subCategoryId), trackChanges)
                .Include(b => b.BookSubCategories)!
                .ThenInclude(bsc => bsc.SubCategory)
                .ToListAsync();
    }
}
