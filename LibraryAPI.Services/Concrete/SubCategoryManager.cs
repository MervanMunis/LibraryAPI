using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.SubCategoryDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;

namespace LibraryAPI.Services.Concrete
{
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public SubCategoryManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubCategoryResponse>> GetAllSubCategoriesAsync(bool trackChanges)
        {
            var subCategories = await _repositoryManager.SubCategoryRepository
                .GetAllSubCategoriesAsync(trackChanges);

            return _mapper.Map<IEnumerable<SubCategoryResponse>>(subCategories);
        }

        public async Task<SubCategoryResponse> GetSubCategoryByIdAsync(short id, bool trackChanges)
        {
            var subCategory = await _repositoryManager.SubCategoryRepository
                .GetSubCategoryByIdAsync(id, trackChanges);

            if (subCategory == null)
                throw new KeyNotFoundException("SubCategory not found");

            return _mapper.Map<SubCategoryResponse>(subCategory);
        }

        public async Task<string> AddSubCategoryAsync(SubCategoryRequest subCategoryRequest)
        {
            var existingSubCategory = await _repositoryManager.SubCategoryRepository
                .GetAllSubCategoriesAsync(false);

            if (existingSubCategory.Any(s => s.Name == subCategoryRequest.Name))
                throw new InvalidOperationException("The subcategory name already exists!");

            var subCategoryEntity = _mapper.Map<SubCategory>(subCategoryRequest);

            await _repositoryManager.SubCategoryRepository.CreateAsync(subCategoryEntity);
            await _repositoryManager.SaveAsync();

            return "SubCategory successfully created!";
        }

        public async Task<bool> UpdateSubCategoryAsync(short id, SubCategoryRequest subCategoryRequest)
        {
            var subCategory = await _repositoryManager.SubCategoryRepository
                .GetSubCategoryByIdAsync(id, true);

            if (subCategory == null)
                throw new KeyNotFoundException("SubCategory not found");

            var existingSubCategory = await _repositoryManager.SubCategoryRepository
                .GetAllSubCategoriesAsync(false);

            if (existingSubCategory.Any(s => s.Name == subCategoryRequest.Name))
                throw new InvalidOperationException("The subcategory name already exists!");

            _mapper.Map(subCategoryRequest, subCategory);

            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> SetSubCategoryStatusAsync(short id, string status)
        {
            var subCategory = await _repositoryManager.SubCategoryRepository
                .GetSubCategoryByIdAsync(id, true);

            if (subCategory == null)
                throw new KeyNotFoundException("SubCategory not found");

            subCategory.SubCategoryStatus = status;
            _repositoryManager.SubCategoryRepository.Update(subCategory);

            if (status == Status.InActive.ToString())
            {
                var bookSubCategories = await _repositoryManager.BookSubCategoryRepository
                    .GetAllBookSubCategoriesAsync(true);

                foreach (var bookSubCategory in bookSubCategories.Where(bsc => bsc.SubCategoriesId == id))
                {
                    var book = await _repositoryManager.BookRepository
                        .GetBookByIdAsync(bookSubCategory.BooksId!.Value, true);

                    if (book != null)
                    {
                        book.BookStatus = status;
                        _repositoryManager.BookRepository.Update(book);
                    }
                }
            }

            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<BookResponse>> GetBooksBySubCategoryIdAsync(short subCategoryId, bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository
                .GetBooksBySubCategoryIdAsync(subCategoryId, trackChanges);

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this subcategory");

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }
    }
}
