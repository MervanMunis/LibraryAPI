using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category> GetCategoryByIdAsync(short categoryId, bool trackChanges);
        //Task<IEnumerable<Category>> GetBooksByCategoryIdAsync(short categoryId, bool trackChanges);
    }
}
