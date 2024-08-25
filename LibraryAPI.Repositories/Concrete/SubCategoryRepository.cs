using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<SubCategory> GetSubCategoryByIdAsync(short subCategoryId, bool trackChanges) =>
            await FindByCondition(sc => sc.SubCategoryId.Equals(subCategoryId), trackChanges).SingleOrDefaultAsync();
    }
}
