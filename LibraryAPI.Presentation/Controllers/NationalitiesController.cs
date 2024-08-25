using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Entities.DTOs.NationalityDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NationalitiesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public NationalitiesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all nationalities.
        /// </summary>
        [HttpGet] // GET: api/Nationalities
        public async Task<ActionResult<IEnumerable<NationalityResponse>>> GetNationalities()
        {
            try
            {
                var nationalities = await _serviceManager.NationalityService.GetAllNationalitiesAsync(trackChanges: false);
                return Ok(nationalities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific nationality by its ID.
        /// </summary>
        [HttpGet("{id}")] // GET: api/Nationalities/{id}
        public async Task<ActionResult<NationalityResponse>> GetNationality(short id)
        {
            try
            {
                var nationality = await _serviceManager.NationalityService.GetNationalityByIdAsync(id, trackChanges: false);
                return Ok(nationality);
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
        /// Retrieves authors by their nationality ID.
        /// </summary>
        [HttpGet("{id}/authors")] // GET: api/Nationalities/{id}/authors
        public async Task<ActionResult<IEnumerable<AuthorResponse>>> GetAuthorsByNationalityId(short id)
        {
            try
            {
                var authors = await _serviceManager.NationalityService.GetAuthorsByNationalityIdAsync(id, trackChanges: false);
                return Ok(authors);
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
        /// Adds a new nationality.
        /// </summary>
        [HttpPost] // POST: api/Nationalities
        public async Task<ActionResult<string>> PostNationality([FromBody] NationalityRequest nationalityRequest)
        {
            try
            {
                await _serviceManager.NationalityService.AddNationalityAsync(nationalityRequest);
                return Ok("Nationality successfully created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing nationality's details.
        /// </summary>
        [HttpPut("{id}")] // PUT: api/Nationalities/{id}
        public async Task<ActionResult<string>> PutNationality(short id, [FromBody] NationalityRequest nationalityRequest)
        {
            try
            {
                await _serviceManager.NationalityService.UpdateNationalityAsync(id, nationalityRequest);
                return Ok("Nationality successfully updated.");
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
        /// Deletes a nationality by its ID.
        /// </summary>
        [HttpDelete("{id}")] // DELETE: api/Nationalities/{id}
        public async Task<ActionResult<string>> DeleteNationality(short id)
        {
            try
            {
                await _serviceManager.NationalityService.DeleteNationalityAsync(id);
                return Ok("Nationality successfully deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
