using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services.Manager;
using LibraryAPI.Entities.DTOs.WantedBookDTO;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WantedBooksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public WantedBooksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all wanted books.
        /// </summary>
        /// <returns>A list of all wanted books.</returns>
        [HttpGet] // GET: api/WantedBooks
        public async Task<ActionResult<IEnumerable<WantedBookResponse>>> GetWantedBooks()
        {
            try
            {
                var wantedBooks = await _serviceManager.WantedBookService.GetAllWantedBooksAsync(trackChanges: false);
                return Ok(wantedBooks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific wanted book by its ID.
        /// </summary>
        /// <param name="id">The ID of the wanted book to retrieve.</param>
        /// <returns>The requested wanted book.</returns>
        [HttpGet("{id}")] // GET: api/WantedBooks/{id}
        public async Task<ActionResult<WantedBookResponse>> GetWantedBook(int id)
        {
            try
            {
                var wantedBook = await _serviceManager.WantedBookService.GetWantedBookByIdAsync(id, trackChanges: false);
                return Ok(wantedBook);
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
        /// Adds a new wanted book.
        /// </summary>
        /// <param name="wantedBookRequest">The details of the wanted book to add.</param>
        /// <returns>A success message if the wanted book is added successfully.</returns>
        [HttpPost] // POST: api/WantedBooks
        public async Task<ActionResult<string>> PostWantedBook([FromBody] WantedBookRequest wantedBookRequest)
        {
            try
            {
                var message = await _serviceManager.WantedBookService.AddWantedBookAsync(wantedBookRequest);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing wanted book.
        /// </summary>
        /// <param name="id">The ID of the wanted book to update.</param>
        /// <param name="wantedBookRequest">The new details of the wanted book.</param>
        /// <returns>A success message if the wanted book is updated successfully.</returns>
        [HttpPut("{id}")] // PUT: api/WantedBooks/{id}
        public async Task<ActionResult<string>> PutWantedBook(int id, [FromBody] WantedBookRequest wantedBookRequest)
        {
            try
            {
                var success = await _serviceManager.WantedBookService.UpdateWantedBookAsync(id, wantedBookRequest);
                if (success)
                    return Ok("Wanted book updated successfully.");
                else
                    return BadRequest("Failed to update wanted book.");
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
        /// Deletes a specific wanted book by its ID.
        /// </summary>
        /// <param name="id">The ID of the wanted book to delete.</param>
        /// <returns>A success message if the wanted book is deleted successfully.</returns>
        [HttpDelete("{id}")] // DELETE: api/WantedBooks/{id}
        public async Task<ActionResult<string>> DeleteWantedBook(int id)
        {
            try
            {
                var success = await _serviceManager.WantedBookService.DeleteWantedBookAsync(id);
                if (success)
                    return Ok("Wanted book deleted successfully.");
                else
                    return BadRequest("Failed to delete wanted book.");
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
