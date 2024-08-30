using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.CategoryDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync(false);
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The details of the category.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> GetCategory(short id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id, false);
            if (category == null)
                return NotFound($"Category with id {id} was not found.");

            return Ok(category);
        }

        /// <summary>
        /// Retrieves all books associated with a specific category.
        /// </summary>
        /// <param name="id">The ID of the category whose books to retrieve.</param>
        /// <returns>A list of books in the specified category.</returns>
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<CategoryBookResponse>>> GetBooksByCategory(short id)
        {
            var books = await _serviceManager.CategoryService.GetBooksByCategoryIdAsync(id, false);
            if (books == null || !books.Any())
                return NotFound($"No books found for category with id {id}.");

            return Ok(books);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="categoryRequest">The request body containing details of the category to add.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPost]
        public async Task<ActionResult<string>> PostCategory([FromBody] CategoryRequest categoryRequest)
        {
            await _serviceManager.CategoryService.AddCategoryAsync(categoryRequest);
            return Ok("The category is successfully created.");
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryRequest">The request body containing updated details of the category.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutCategory(short id, [FromBody] CategoryRequest categoryRequest)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id, trackChanges: true);
            if (category == null)
                return NotFound($"Category with id {id} was not found.");

            await _serviceManager.CategoryService.UpdateCategoryAsync(id, categoryRequest);
            return Ok("The category is updated successfully.");
        }

        /// <summary>
        /// Sets a category to inactive status.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPatch("{id}/status/inactive")]
        public async Task<ActionResult<string>> InActiveCategory(short id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id, trackChanges: true);
            if (category == null)
                return NotFound($"Category with id {id} was not found.");

            await _serviceManager.CategoryService.SetCategoryStatusAsync(id, Status.InActive.ToString());
            return Ok("The category is set to inactive.");
        }

        /// <summary>
        /// Sets a category to active status.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPatch("{id}/status/active")]
        public async Task<ActionResult<string>> SetCategoryActiveStatus(short id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id, trackChanges: true);
            if (category == null)
                return NotFound($"Category with id {id} was not found.");

            await _serviceManager.CategoryService.SetCategoryStatusAsync(id, Status.Active.ToString());
            return Ok("The category is set to active.");
        }
    }
}
