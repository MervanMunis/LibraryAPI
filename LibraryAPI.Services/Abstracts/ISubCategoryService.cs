using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.SubCategoryDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryResponse>> GetAllSubCategoriesAsync(bool trackChanges);
        Task<SubCategoryResponse> GetSubCategoryByIdAsync(short id, bool trackChanges);
        Task<string> AddSubCategoryAsync(SubCategoryRequest subCategoryRequest);
        Task<bool> UpdateSubCategoryAsync(short id, SubCategoryRequest subCategoryRequest);
        Task<bool> SetSubCategoryStatusAsync(short id, string status);
        Task<IEnumerable<BookResponse>> GetBooksBySubCategoryIdAsync(short subCategoryId, bool trackChanges);
    }
}
