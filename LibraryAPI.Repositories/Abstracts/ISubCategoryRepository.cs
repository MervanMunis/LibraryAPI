using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync(bool trackChanges);
        Task<SubCategory> GetSubCategoryByIdAsync(short subCategoryId, bool trackChanges);
    }
}
