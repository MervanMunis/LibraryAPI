using LibraryAPI.Entities.DTOs.LoanDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LoansController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("employee/{id}")] // GET: api/Loans/employee
        public async Task<ActionResult<IEnumerable<LoanResponse>>> GetLoansByEmployeeId([FromRoute(Name = "id")] string employeeId)
        {
            // var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var loans = await _serviceManager.LoanService.GetLoansByEmployeeIdAsync(employeeId, trackChanges: false);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Changed from Problem to BadRequest
            }
        }

        [HttpGet("member/{id}")] // GET: api/Loans/member
        public async Task<ActionResult<IEnumerable<LoanResponse>>> GetLoansByMemberId([FromRoute(Name = "id")] string memberId)
        {
            // var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var loans = await _serviceManager.LoanService.GetLoansByMemberIdAsync(memberId, trackChanges: false);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Changed from Problem to BadRequest
            }
        }

        [HttpGet("{id}")] // GET: api/Loans/{id}
        public async Task<ActionResult<LoanResponse>> GetLoan(int id)
        {
            try
            {
                var loan = await _serviceManager.LoanService.GetLoanByIdAsync(id, trackChanges: false);
                return Ok(loan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{loanId}/transactions")] // GET: api/Loans/{loanId}/transactions
        public async Task<ActionResult<IEnumerable<LoanTransactionResponse>>> GetLoanTransactionByLoanId(int loanId)
        {
            try
            {
                var transactions = await _serviceManager.LoanService.GetLoanTransactionByLoanIdAsync(loanId, trackChanges: false);
                return Ok(transactions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}")] // POST: api/Loans
        public async Task<ActionResult<string>> PostLoan([FromRoute(Name = "id")] string id, [FromBody] LoanRequest loanRequest)
        {
            try
            {
                loanRequest.EmployeeId = id;
                // loanRequest.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _serviceManager.LoanService.AddLoanAsync(loanRequest);
                return Ok("Loan created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")] // PUT: api/Loans/{id}
        public async Task<ActionResult<string>> PutLoan(int id, [FromBody] LoanUpdateRequest loanUpdateRequest)
        {
            try
            {
                loanUpdateRequest.LoanId = id;
                await _serviceManager.LoanService.UpdateLoanAsync(loanUpdateRequest);
                return Ok("Loan status updated successfully.");
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

        [HttpPatch("{id}/return")] // PATCH: api/Loans/{id}/return
        public async Task<ActionResult<string>> ReturnBook(int id)
        {
            try
            {
                await _serviceManager.LoanService.ReturnBookAsync(id);
                return Ok("Book returned successfully.");
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
