using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.CategoryDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public CategoryManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(bool trackChanges)
        {
            var categories = await _repositoryManager.CategoryRepository.GetAllCategoriesAsync(trackChanges);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(short id, bool trackChanges)
        {
            var category = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id, trackChanges);
            if (category == null || category.CategoryStatus != Status.Active.ToString())
                throw new KeyNotFoundException("Category not found");

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task AddCategoryAsync(CategoryRequest categoryRequest)
        {
            var existingCategory = await _repositoryManager.CategoryRepository
                .FindByCondition(c => c.Name == categoryRequest.Name, false)
                .FirstOrDefaultAsync();

            if (existingCategory != null)
                throw new InvalidOperationException("The category name already exists!");

            var category = _mapper.Map<Category>(categoryRequest);

            await _repositoryManager.CategoryRepository.CreateAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCategoryAsync(short id, CategoryRequest categoryRequest)
        {
            var existingCategory = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id, true);
            if (existingCategory == null)
                throw new KeyNotFoundException("Category not found");

            var categoryWithSameName = await _repositoryManager.CategoryRepository
                .FindByCondition(c => c.Name == categoryRequest.Name && c.CategoryId != id, false)
                .FirstOrDefaultAsync();

            if (categoryWithSameName != null)
                throw new InvalidOperationException("The category name already exists!");

            existingCategory.Name = categoryRequest.Name;
            _repositoryManager.CategoryRepository.Update(existingCategory);
            await _repositoryManager.SaveAsync();
        }

        public async Task SetCategoryStatusAsync(short id, string status)
        {
            var category = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id, true);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            category.CategoryStatus = status;
            _repositoryManager.CategoryRepository.Update(category);

            var subCategories = await _repositoryManager.SubCategoryRepository
                .FindByCondition(sc => sc.CategoryId == id, true)
                .ToListAsync();

            foreach (var subCategory in subCategories)
            {
                subCategory.SubCategoryStatus = status;
                _repositoryManager.SubCategoryRepository.Update(subCategory);

                if (status == Status.InActive.ToString())
                {
                    var bookSubCategories = await _repositoryManager.BookSubCategoryRepository
                        .FindByCondition(bsc => bsc.SubCategoriesId == subCategory.SubCategoryId, true)
                        .Include(bsc => bsc.Book)
                        .ToListAsync();

                    foreach (var bookSubCategory in bookSubCategories)
                    {
                        bookSubCategory.Book!.BookStatus = status;
                        _repositoryManager.BookRepository.Update(bookSubCategory.Book);
                    }
                }
            }

            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<CategoryBookResponse>> GetBooksByCategoryIdAsync(short categoryId, bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository
                .FindByCondition(b => b.BookSubCategories!.Any(bsc => bsc.SubCategory!.CategoryId == categoryId), trackChanges)
                .Select(b => new CategoryBookResponse
                {
                    BookId = b.BookId,
                    ISBN = b.ISBN,
                    Title = b.Title,
                    SubCategoryNames = b.BookSubCategories!.Select(bs => bs.SubCategory!.Name).ToList(),
                })
                .ToListAsync();

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this category");

            return books;
        }
    }
}
