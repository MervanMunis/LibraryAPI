using LibraryAPI.Entities.DTOs.PenaltyDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenaltiesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PenaltiesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all penalties associated with the currently logged-in member.
        /// </summary>
        /// <returns>A list of penalties for the current member.</returns>
        [HttpGet("member/{id}")] // GET: api/Penalties/member
        public async Task<ActionResult<IEnumerable<PenaltyResponse>>> GetPenaltiesByMemberId([FromRoute(Name = "id")] string memberId)
        {
            try
            {
                // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var penalties = await _serviceManager.PenaltyService.GetPenaltiesByMemberIdAsync(memberId, trackChanges: false);

                return Ok(penalties);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific penalty by its ID.
        /// </summary>
        /// <param name="id">The ID of the penalty.</param>
        /// <returns>The details of the penalty.</returns>
        [HttpGet("{id}")] // GET: api/Penalties/{id}
        public async Task<ActionResult<PenaltyResponse>> GetPenalty(long id)
        {
            try
            {
                var penalty = await _serviceManager.PenaltyService.GetPenaltyByIdAsync(id, trackChanges: false);
                return Ok(penalty);
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
