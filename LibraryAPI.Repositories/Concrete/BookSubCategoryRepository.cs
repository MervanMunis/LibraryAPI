using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookSubCategoryRepository : RepositoryBase<BookSubCategory>, IBookSubCategoryRepository
    {
        public BookSubCategoryRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<BookSubCategory>> GetAllBookSubCategoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<BookSubCategory> GetBookSubCategoryByIdAsync(long bookId, short subCategoryId, bool trackChanges) =>
            await FindByCondition(bsc => bsc.BooksId == bookId && bsc.SubCategoriesId == subCategoryId, trackChanges).SingleOrDefaultAsync();
    }
}
