using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        // Make Category as nested category dont need to use subcategory.
        // public async Task<IEnumerable<Category>> GetBooksByCategoryIdAsync(short categoryId, bool trackChanges)
           // => await FindAll(trackChanges).Include(b => b.Bo)

        public async Task<Category> GetCategoryByIdAsync(short categoryId, bool trackChanges) =>
            await FindByCondition(c => c.CategoryId.Equals(categoryId), trackChanges).SingleOrDefaultAsync();
    }
}
