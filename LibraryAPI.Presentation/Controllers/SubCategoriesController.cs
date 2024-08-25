using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services.Manager;
using LibraryAPI.Entities.DTOs.SubCategoryDTO;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SubCategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all subcategories.
        /// </summary>
        /// <returns>A list of all subcategories.</returns>
        [HttpGet] // GET: api/SubCategories
        public async Task<ActionResult<IEnumerable<SubCategoryResponse>>> GetSubCategories()
        {
            try
            {
                var subCategories = await _serviceManager.SubCategoryService.GetAllSubCategoriesAsync(trackChanges: false);
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific subcategory by its ID.
        /// </summary>
        /// <param name="id">The ID of the subcategory to retrieve.</param>
        /// <returns>The requested subcategory.</returns>
        [HttpGet("{id}")] // GET: api/SubCategories/{id}
        public async Task<ActionResult<SubCategoryResponse>> GetSubCategory(short id)
        {
            try
            {
                var subCategory = await _serviceManager.SubCategoryService.GetSubCategoryByIdAsync(id, trackChanges: false);
                return Ok(subCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all books associated with a specific subcategory.
        /// </summary>
        /// <param name="id">The ID of the subcategory whose books are to be retrieved.</param>
        /// <returns>A list of books associated with the subcategory.</returns>
        [HttpGet("{id}/books")] // GET: api/SubCategories/{id}/books
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksBySubCategory(short id)
        {
            try
            {
                var books = await _serviceManager.SubCategoryService.GetBooksBySubCategoryIdAsync(id, trackChanges: false);
                return Ok(books);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new subcategory.
        /// </summary>
        /// <param name="subCategoryRequest">The details of the subcategory to add.</param>
        /// <returns>A success message if the subcategory is added successfully.</returns>
        [HttpPost] // POST: api/SubCategories
        public async Task<ActionResult<string>> PostSubCategory([FromBody] SubCategoryRequest subCategoryRequest)
        {
            try
            {
                var message = await _serviceManager.SubCategoryService.AddSubCategoryAsync(subCategoryRequest);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing subcategory.
        /// </summary>
        /// <param name="id">The ID of the subcategory to update.</param>
        /// <param name="subCategoryRequest">The new details of the subcategory.</param>
        /// <returns>A success message if the subcategory is updated successfully.</returns>
        [HttpPut("{id}")] // PUT: api/SubCategories/{id}
        public async Task<ActionResult<string>> PutSubCategory(short id, [FromBody] SubCategoryRequest subCategoryRequest)
        {
            try
            {
                var success = await _serviceManager.SubCategoryService.UpdateSubCategoryAsync(id, subCategoryRequest);
                if (success)
                    return Ok("SubCategory updated successfully.");
                else
                    return BadRequest("Failed to update SubCategory.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets a subcategory to inactive status.
        /// </summary>
        /// <param name="id">The ID of the subcategory to set to inactive.</param>
        /// <returns>A success message if the subcategory is set to inactive successfully.</returns>
        [HttpPatch("{id}/status/inactive")] // PATCH: api/SubCategories/{id}/status/inactive
        public async Task<ActionResult<string>> InActiveSubCategory(short id)
        {
            try
            {
                var success = await _serviceManager.SubCategoryService.SetSubCategoryStatusAsync(id, Status.InActive.ToString());
                if (success)
                    return Ok("SubCategory set to inactive.");
                else
                    return BadRequest("Failed to set SubCategory to inactive.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets a subcategory to active status.
        /// </summary>
        /// <param name="id">The ID of the subcategory to set to active.</param>
        /// <returns>A success message if the subcategory is set to active successfully.</returns>
        [HttpPatch("{id}/status/active")] // PATCH: api/SubCategories/{id}/status/active
        public async Task<ActionResult<string>> ActiveSubCategory(short id)
        {
            try
            {
                var success = await _serviceManager.SubCategoryService.SetSubCategoryStatusAsync(id, Status.Active.ToString());
                if (success)
                    return Ok("SubCategory set to active.");
                else
                    return BadRequest("Failed to set SubCategory to active.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
