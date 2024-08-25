using LibraryAPI.Entities.DTOs.PublisherDTO;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.PublisherAddressDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PublishersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all publishers.
        /// </summary>
        /// <returns>A list of publishers.</returns>
        [HttpGet] // GET: api/Publishers
        public async Task<ActionResult<IEnumerable<PublisherResponse>>> GetPublishers()
        {
            try
            {
                var publishers = await _serviceManager.PublisherService.GetAllPublishersAsync(trackChanges: false);
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific publisher by ID.
        /// </summary>
        /// <param name="id">The ID of the publisher.</param>
        /// <returns>The details of the publisher.</returns>
        [HttpGet("{id}")] // GET: api/Publishers/{id}
        public async Task<ActionResult<PublisherResponse>> GetPublisher(long id)
        {
            try
            {
                var publisher = await _serviceManager.PublisherService.GetPublisherByIdAsync(id, trackChanges: false);
                return Ok(publisher);
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
        /// Retrieves all books published by a specific publisher.
        /// </summary>
        /// <param name="id">The ID of the publisher.</param>
        /// <returns>A list of books published by the specified publisher.</returns>
        [HttpGet("{id}/books")] // GET: api/Publishers/{id}/books
        public async Task<ActionResult<IEnumerable<CategoryBookResponse>>> GetBooksByPublisher(long id)
        {
            try
            {
                var books = await _serviceManager.PublisherService.GetBooksByPublisherIdAsync(id, trackChanges: false);
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
        /// Retrieves the address of a specific publisher.
        /// </summary>
        /// <param name="id">The ID of the publisher.</param>
        /// <returns>The address details of the specified publisher.</returns>
        [HttpGet("{id}/address")] // GET: api/Publishers/{id}/address
        public async Task<ActionResult<PublisherAddressResponse>> GetPublisherAddress(int id)
        {
            try
            {
                var address = await _serviceManager.PublisherService.GetPublisherAddressAsync(id, trackChanges: false);
                return Ok(address);
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
        /// Adds a new publisher.
        /// </summary>
        /// <param name="publisherRequest">The details of the publisher to be added.</param>
        /// <returns>A success message if the publisher is added successfully.</returns>
        [HttpPost] // POST: api/Publishers
        public async Task<ActionResult<string>> PostPublisher([FromBody] PublisherRequest publisherRequest)
        {
            try
            {
                var message = await _serviceManager.PublisherService.AddPublisherAsync(publisherRequest);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing publisher's details.
        /// </summary>
        /// <param name="id">The ID of the publisher to be updated.</param>
        /// <param name="publisherRequest">The new details of the publisher.</param>
        /// <returns>A success message if the publisher is updated successfully.</returns>
        [HttpPut("{id}")] // PUT: api/Publishers/{id}
        public async Task<ActionResult<string>> PutPublisher(long id, [FromBody] PublisherRequest publisherRequest)
        {
            try
            {
                var success = await _serviceManager.PublisherService.UpdatePublisherAsync(id, publisherRequest);
                if (success)
                    return Ok("Publisher updated successfully.");
                else
                    return BadRequest("Failed to update publisher.");
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
        /// Sets a publisher's status to active.
        /// </summary>
        /// <param name="id">The ID of the publisher whose status is to be set to active.</param>
        /// <returns>A success message if the publisher is set to active status.</returns>
        [HttpPatch("{id}/status/active")] // PATCH: api/Publishers/{id}/status/active
        public async Task<ActionResult<string>> ActivePublisher(long id)
        {
            try
            {
                var success = await _serviceManager.PublisherService.SetPublisherStatusAsync(id, "Active");
                if (success)
                    return Ok("Publisher set to active.");
                else
                    return BadRequest("Failed to set publisher to active.");
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
        /// Sets a publisher's status to inactive.
        /// </summary>
        /// <param name="id">The ID of the publisher whose status is to be set to inactive.</param>
        /// <returns>A success message if the publisher is set to inactive status.</returns>
        [HttpPatch("{id}/status/inactive")] // PATCH: api/Publishers/{id}/status/inactive
        public async Task<ActionResult<string>> InActivePublisher(long id)
        {
            try
            {
                var success = await _serviceManager.PublisherService.SetPublisherStatusAsync(id, "InActive");
                if (success)
                    return Ok("Publisher set to inactive.");
                else
                    return BadRequest("Failed to set publisher to inactive.");
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
        /// Adds or updates the address of a specific publisher.
        /// </summary>
        /// <param name="id">The ID of the publisher whose address is to be added or updated.</param>
        /// <param name="address">The new address details of the publisher.</param>
        /// <returns>A success message if the address is added or updated successfully.</returns>
        [HttpPut("{id}/address")] // PUT: api/Publishers/{id}/address
        public async Task<ActionResult<string>> AddOrUpdatePublisherAddress(long id, [FromBody] PublisherAddressRequest address)
        {
            try
            {
                var message = await _serviceManager.PublisherService.AddOrUpdatePublisherAddressAsync(id, address);
                return Ok(message);
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
