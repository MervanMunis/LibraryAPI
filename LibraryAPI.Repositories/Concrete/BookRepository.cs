using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) =>
            await IncludeRelatedEntities(FindAll(trackChanges)).ToListAsync();

        public async Task<IEnumerable<Book>> GetAllActiveBooksAsync(string active, bool trackChanges)
            => await IncludeRelatedEntities(FindByCondition(a => a.BookStatus.Equals(active), trackChanges)).ToListAsync();

        public async Task<IEnumerable<Book>> GetAllBannedBooksAsync(string banned, bool trackChanges)
            => await IncludeRelatedEntities(FindByCondition(a => a.BookStatus.Equals(banned), trackChanges)).ToListAsync();

        public async Task<IEnumerable<Book>> GetAllInActiveBooksAsync(string inActive, bool trackChanges)
            => await IncludeRelatedEntities(FindByCondition(a => a.BookStatus.Equals(inActive), trackChanges)).ToListAsync();

        public async Task<Book> GetBookByIdAsync(long bookId, bool trackChanges) =>
            await IncludeRelatedEntities(FindByCondition(b => b.BookId.Equals(bookId), trackChanges)).SingleOrDefaultAsync();

        public async Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(long publisherId, bool trackChanges) => 
            await IncludeRelatedEntities(FindByCondition(b => b.PublisherId == publisherId, trackChanges)).ToListAsync();

        public async Task<IEnumerable<Book>> GetBooksBySubCategoryIdAsync(short subCategoryId, bool trackChanges) =>
            await IncludeRelatedEntities(FindByCondition(b => 
            b.BookSubCategories!.Any(bsc => bsc.SubCategoriesId == subCategoryId), trackChanges))
                .ToListAsync();

        private IQueryable<Book> IncludeRelatedEntities(IQueryable<Book> query)
        {
            return query
                .Include(b => b.BookSubCategories)!
                    .ThenInclude(bsc => bsc.SubCategory)
                    .ThenInclude(sc => sc!.Category)
                .Include(b => b.BookLanguages)!
                    .ThenInclude(bl => bl.Language)
                .Include(b => b.AuthorBooks)!
                    .ThenInclude(ab => ab.Author)
                .Include(b => b.Publisher);
        }
    }
}
