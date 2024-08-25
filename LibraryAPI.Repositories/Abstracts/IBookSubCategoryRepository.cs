using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IBookSubCategoryRepository : IRepositoryBase<BookSubCategory>
    {
        Task<IEnumerable<BookSubCategory>> GetAllBookSubCategoriesAsync(bool trackChanges);
        Task<BookSubCategory> GetBookSubCategoryByIdAsync(long bookId, short subCategoryId, bool trackChanges);
    }
}
