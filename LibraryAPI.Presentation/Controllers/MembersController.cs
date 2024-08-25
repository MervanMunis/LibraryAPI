using LibraryAPI.Entities.DTOs.MemberDTO;
using LibraryAPI.Entities.DTOs.PasswordDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MembersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all members from the database.
        /// </summary>
        [HttpGet] // GET: api/Members
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembers()
        {
            try
            {
                var members = await _serviceManager.MemberService.GetAllMembersAsync(trackChanges: false);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific member by their ID (the currently logged-in member).
        /// </summary>
        [HttpGet("memberById/{id}")] // GET: api/Members/memberById
        public async Task<ActionResult<MemberResponse>> GetMember([FromRoute(Name = "id")] string memberId)
        {
            try
            {
                //var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var member = await _serviceManager.MemberService.GetMemberByIdAsync(memberId, trackChanges: false);
                return Ok(member);
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
        /// Retrieves a specific member by their ID number.
        /// </summary>
        [HttpGet("{idNumber}")] // GET: api/Members/{idNumber}
        public async Task<ActionResult<MemberResponse>> GetMemberByIdNumber(string idNumber)
        {
            try
            {
                var member = await _serviceManager.MemberService.GetMemberByIdNumberAsync(idNumber, trackChanges: false);
                return Ok(member);
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
        /// Adds a new member to the database.
        /// </summary>
        [HttpPost] // POST: api/Members
        public async Task<ActionResult<string>> PostMember([FromBody] MemberRequest memberRequest)
        {
            try
            {
                await _serviceManager.MemberService.AddMemberAsync(memberRequest);
                return Ok("Welcome to the library.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing member's details (the currently logged-in member).
        /// </summary>
        [HttpPut("{id}")] // PUT: api/Members
        public async Task<ActionResult<string>> PutMember([FromRoute(Name = "id")] string memberId, [FromBody] MemberRequest memberRequest)
        {
            try
            {
                // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _serviceManager.MemberService.UpdateMemberAsync(memberId, memberRequest);
                return Ok("The member is updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes a member by setting their status to removed (the currently logged-in member).
        /// </summary>
        [HttpPatch("Remove/{id}")] // PATCH: api/Members/Remove
        public async Task<ActionResult<string>> RemoveMember([FromRoute(Name = "id")] string memberId)
        {
            try
            {
                // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _serviceManager.MemberService.SetMemberStatusAsync(memberId, MemberStatus.RemovedAccount.ToString());
                return Ok("The member account is set to removed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets the status of a member to blocked by their ID number.
        /// </summary>
        [HttpPatch("{idNumber}/status/blocked")] // PATCH: api/Members/{idNumber}/status/blocked
        public async Task<ActionResult<string>> SetMemberBlockedStatus(string idNumber)
        {
            try
            {
                await _serviceManager.MemberService.SetMemberStatusAsync(idNumber, MemberStatus.BlockedAccount.ToString());
                return Ok("The member account is set to blocked.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates a member's password (the currently logged-in member).
        /// </summary>
        [HttpPatch("password/{id}")] // PATCH: api/Members/password
        public async Task<ActionResult<string>> UpdateMemberPassword([FromRoute(Name = "id")] string memberId, [FromBody] UpdatePasswordRequest updatePasswordDTO)
        {
            try
            {
                // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _serviceManager.MemberService.UpdateMemberPasswordAsync(memberId, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);
                return Ok("The password is updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new address for a member (the currently logged-in member).
        /// </summary>
        [HttpPost("address/{id}")] // POST: api/Members/address
        public async Task<ActionResult<string>> PostMemberAddress([FromRoute(Name = "id")] string memberId,[FromBody] MemberAddress memberAddress)
        {
            try
            {
                //memberAddress.MemberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                memberAddress.MemberId = memberId;

                await _serviceManager.MemberService.AddMemberAddressAsync(memberAddress);
                return Ok("The member's address is added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
