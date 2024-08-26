using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthorsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all authors.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                bool trackChanges = false;
                var authors = await _serviceManager.AuthorService.GetAllAuthorsAsync(trackChanges);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all active authors.
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAllActiveAuthors()
        {
            try
            {
                bool trackChanges = false;
                var authors = await _serviceManager.AuthorService.GetAllActiveAuthorsAsync(trackChanges);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all inactive authors.
        /// </summary>
        [HttpGet("inactive")]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAllInactiveAuthors()
        {
            try
            {
                bool trackChanges = false;
                var authors = await _serviceManager.AuthorService.GetAllInActiveAuthorsAsync(trackChanges);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all banned authors.
        /// </summary>
        [HttpGet("banned")]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAllBannedAuthors()
        {
            try
            {
                bool trackChanges = false;
                var authors = await _serviceManager.AuthorService.GetAllBannedAuthorsAsync(trackChanges);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an author by their ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAuthor(long id)
        {
            try
            {
                bool trackChanges = false;
                var author = await _serviceManager.AuthorService.GetAuthorByIdAsync(id, trackChanges);
                return author == null ? NotFound("Author not found") : Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new author.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> PostAuthor([FromBody] AuthorRequest authorRequest)
        {
            if (authorRequest == null)
                return BadRequest("Author request cannot be null.");

            try
            {
                var message = await _serviceManager.AuthorService.AddAuthorAsync(authorRequest);
                return CreatedAtAction(nameof(GetAuthor), new { id = authorRequest.FullName }, message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing author's details.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> PutAuthor(long id, [FromBody] AuthorRequest authorRequest)
        {
            if (authorRequest == null)
                return BadRequest("Author request cannot be null.");

            try
            {
                var success = await _serviceManager.AuthorService.UpdateAuthorAsync(id, authorRequest, true);
                return success ? Ok("Author updated successfully.") : NotFound("Author not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sets an author to inactive.
        /// </summary>
        [HttpPatch("{id}/status/inactive")]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> InactiveAuthor(long id)
        {
            try
            {
                var success = await _serviceManager.AuthorService.SetAuthorStatusAsync(id, "InActive");
                return success ? Ok("Author set to inactive.") : NotFound("Author not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Bans an author.
        /// </summary>
        [HttpPatch("{id}/status/banned")]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> BanAuthor(long id)
        {
            try
            {
                var success = await _serviceManager.AuthorService.SetAuthorStatusAsync(id, "Banned");
                return success ? Ok("Author banned successfully.") : NotFound("Author not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sets an author to active.
        /// </summary>
        [HttpPatch("{id}/status/active")]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> SetAuthorActive(long id)
        {
            try
            {
                var success = await _serviceManager.AuthorService.SetAuthorStatusAsync(id, "Active");
                return success ? Ok("Author set to active.") : NotFound("Author not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the image of an author.
        /// </summary>
        [HttpPatch("{id}/image")]
        [Authorize(Roles = "Librarian, HeadOfLibrary")]
        public async Task<IActionResult> UpdateAuthorImage(long id, IFormFile image)
        {
            if (image == null)
                return BadRequest("Image file is required.");

            try
            {
                var success = await _serviceManager.AuthorService.UpdateAuthorImageAsync(id, image);
                return success ? Ok("Author image updated successfully.") : NotFound("Author not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the image of an author by author ID.
        /// </summary>
        [HttpGet("{id}/image")]
        [Authorize(Roles = "Member, Librarian, HeadOfLibrary")]
        public async Task<IActionResult> GetAuthorImage(long id)
        {
            try
            {
                var image = await _serviceManager.AuthorService.GetAuthorImageAsync(id);
                if (image == null)
                    return NotFound("Image not found.");

                return File(image, "image/jpeg"); // Adjust MIME type as necessary.
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
