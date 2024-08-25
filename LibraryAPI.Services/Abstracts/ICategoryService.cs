using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.CategoryDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(bool trackChanges);

        Task<CategoryResponse> GetCategoryByIdAsync(short id, bool trackChanges);

        Task AddCategoryAsync(CategoryRequest categoryRequest);

        Task UpdateCategoryAsync(short id, CategoryRequest categoryRequest);

        Task SetCategoryStatusAsync(short id, string status);

        Task<IEnumerable<CategoryBookResponse>> GetBooksByCategoryIdAsync(short categoryId, bool trackChanges);
    }
}
