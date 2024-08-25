using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LocationDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LocationsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all active locations.
        /// </summary>
        [HttpGet] // GET: api/Locations
        public async Task<ActionResult<IEnumerable<LocationResponse>>> GetLocations()
        {
            try
            {
                var locations = await _serviceManager.LocationService.GetAllLocationsAsync(trackChanges: false);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific location by its ID.
        /// </summary>
        [HttpGet("{id}")] // GET: api/Locations/{id}
        public async Task<ActionResult<LocationResponse>> GetLocation(int id)
        {
            try
            {
                var location = await _serviceManager.LocationService.GetLocationByIdAsync(id, trackChanges: false);
                return Ok(location);
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
        /// Adds a new location to the database.
        /// </summary>
        [HttpPost] // POST: api/Locations
        public async Task<ActionResult<string>> PostLocation([FromBody] LocationRequest locationRequest)
        {
            try
            {
                await _serviceManager.LocationService.AddLocationAsync(locationRequest);
                return Ok("Location successfully created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing location's details.
        /// </summary>
        [HttpPut("{id}")] // PUT: api/Locations/{id}
        public async Task<ActionResult<string>> PutLocation(int id, [FromBody] LocationRequest locationRequest)
        {
            try
            {
                await _serviceManager.LocationService.UpdateLocationAsync(id, locationRequest);
                return Ok("Location successfully updated.");
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
        /// Sets the status of a location to inactive.
        /// </summary>
        [HttpPatch("{id}/status/inactive")] // PATCH: api/Locations/{id}/status/inactive
        public async Task<ActionResult<string>> InActiveLocation(int id)
        {
            try
            {
                await _serviceManager.LocationService.SetLocationStatusAsync(id, Status.InActive.ToString());
                return Ok("Location is set to inactive.");
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

        /// <summary>
        /// Sets the status of a location to active.
        /// </summary>
        [HttpPatch("{id}/status/active")] // PATCH: api/Locations/{id}/status/active
        public async Task<ActionResult<string>> ActiveLocation(int id)
        {
            try
            {
                await _serviceManager.LocationService.SetLocationStatusAsync(id, Status.Active.ToString());
                return Ok("Location is set to active.");
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
        /// Retrieves books by section code.
        /// </summary>
        [HttpGet("section/{sectionCode}")] // GET: api/Locations/section/{sectionCode}
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksBySectionCode(string sectionCode)
        {
            try
            {
                var books = await _serviceManager.LocationService.GetBooksBySectionCodeAsync(sectionCode, trackChanges: false);
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
        /// Retrieves books by aisle code.
        /// </summary>
        [HttpGet("aisle/{aisleCode}")] // GET: api/Locations/aisle/{aisleCode}
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByAisleCode(string aisleCode)
        {
            try
            {
                var books = await _serviceManager.LocationService.GetBooksByAisleCodeAsync(aisleCode, trackChanges: false);
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
        /// Retrieves books by shelf number.
        /// </summary>
        [HttpGet("shelf/{shelfNumber}")] // GET: api/Locations/shelf/{shelfNumber}
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByShelfNumber(string shelfNumber)
        {
            try
            {
                var books = await _serviceManager.LocationService.GetBooksByShelfNumberAsync(shelfNumber, trackChanges: false);
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
    }
}
