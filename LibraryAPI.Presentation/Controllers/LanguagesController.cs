using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LanguageDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguagesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LanguagesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all active languages.
        /// </summary>
        /// <returns>A list of active languages.</returns>
        [HttpGet] // GET: api/Languages
        public async Task<ActionResult<IEnumerable<LanguageResponse>>> GetLanguages()
        {
            var languages = await _serviceManager.LanguageService.GetAllLanguagesAsync(false);
            return Ok(languages);
        }

        /// <summary>
        /// Retrieves a specific language by its ID.
        /// </summary>
        /// <param name="id">The ID of the language.</param>
        /// <returns>The language details.</returns>
        [HttpGet("{id}")] // GET: api/Languages/{id}
        public async Task<ActionResult<LanguageResponse>> GetLanguage(short id)
        {
            try
            {
                var language = await _serviceManager.LanguageService.GetLanguageByIdAsync(id, false);
                return Ok(language);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves books associated with a specific language.
        /// </summary>
        /// <param name="id">The ID of the language.</param>
        /// <returns>A list of books associated with the language.</returns>
        [HttpGet("{id}/books")] // GET: api/Languages/{id}/books
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByLanguage(short id)
        {
            try
            {
                var books = await _serviceManager.LanguageService.GetBooksByLanguageIdAsync(id, false);
                return Ok(books);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new language to the database.
        /// </summary>
        /// <param name="languageRequest">The language details.</param>
        /// <returns>A success message if the language is created successfully.</returns>
        [HttpPost] // POST: api/Languages
        public async Task<ActionResult<string>> PostLanguage([FromBody] LanguageRequest languageRequest)
        {
            try
            {
                await _serviceManager.LanguageService.AddLanguageAsync(languageRequest);
                return Ok("Language successfully created.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing language's details.
        /// </summary>
        /// <param name="id">The ID of the language to update.</param>
        /// <param name="languageRequest">The updated language details.</param>
        /// <returns>A success message if the language is updated successfully.</returns>
        [HttpPut("{id}")] // PUT: api/Languages/{id}
        public async Task<ActionResult<string>> PutLanguage(short id, [FromBody] LanguageRequest languageRequest)
        {
            try
            {
                await _serviceManager.LanguageService.UpdateLanguageAsync(id, languageRequest);
                return Ok("Language successfully updated.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets the language and its associated books to active status.
        /// </summary>
        /// <param name="id">The ID of the language.</param>
        /// <returns>A success message if the language is set to active status.</returns>
        [HttpPatch("{id}/status/active")] // PATCH: api/Languages/{id}/status/active
        public async Task<ActionResult<string>> SetLanguageActiveStatus(short id)
        {
            try
            {
                await _serviceManager.LanguageService.SetLanguageStatusAsync(id, Status.Active.ToString());
                return Ok("The language and its books are set to active status.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets the language and its associated books to inactive status.
        /// </summary>
        /// <param name="id">The ID of the language.</param>
        /// <returns>A success message if the language is set to inactive status.</returns>
        [HttpPatch("{id}/status/inactive")] // PATCH: api/Languages/{id}/status/inactive
        public async Task<ActionResult<string>> InActiveLanguage(short id)
        {
            try
            {
                await _serviceManager.LanguageService.SetLanguageStatusAsync(id, Status.InActive.ToString());
                return Ok("The language and its books are set to inactive status.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
