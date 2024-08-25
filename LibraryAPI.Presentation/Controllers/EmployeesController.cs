using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.DTOs.PasswordDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of employee responses.</returns>
        [HttpGet] // GET: /api/Employees
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetEmployees(bool trackChanges = false)
        {
            var employees = await _serviceManager.EmployeeService.GetAllEmployeesAsync(trackChanges);
            return Ok(employees);
        }

        /// <summary>
        /// Retrieves a specific employee by their ID number.
        /// </summary>
        /// <param name="idNumber">The ID number of the employee.</param>
        /// <returns>An employee response if found, otherwise an error message.</returns>
        [HttpGet("{idNumber}")] // GET: /api/Employees/{idNumber}
        public async Task<ActionResult<EmployeeResponse>> GetEmployee(string idNumber, bool trackChanges = false)
        {
            try
            {
                var employee = await _serviceManager.EmployeeService.GetEmployeeByIdNumberAsync(idNumber, trackChanges);
                return Ok(employee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// <param name="employeeRequest">The employee request data.</param>
        /// <returns>A success message if the employee is created successfully.</returns>
        [HttpPost] // POST: /api/Employees
        public async Task<ActionResult<string>> PostEmployee([FromBody] EmployeeRequest employeeRequest)
        {
            try
            {
                await _serviceManager.EmployeeService.AddEmployeeAsync(employeeRequest);
                return Ok("Welcome to the club.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing employee's details.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employeeRequest">The updated employee request data.</param>
        /// <returns>A success message if the employee is updated successfully.</returns>
        [HttpPut("{id}")] // PUT: /api/Employees/{id}
        public async Task<ActionResult<string>> PutEmployee(string id, [FromBody] EmployeeRequest employeeRequest)
        {
            try
            {
                await _serviceManager.EmployeeService.UpdateEmployeeAsync(id, employeeRequest);
                return Ok("The employee is updated successfully.");
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
        /// Sets an employee's status to active.
        /// </summary>
        /// <param name="id">The ID of the employee whose status is to be updated.</param>
        /// <returns>A success message if the employee's status is updated successfully.</returns>
        [HttpPatch("{id}/status/active")] // PATCH: /api/Employees/{id}/status/active
        public async Task<ActionResult<string>> SetEmployeeActiveStatus(string id)
        {
            try
            {
                await _serviceManager.EmployeeService.SetEmployeeStatusAsync(id, EmployeeStatus.Working.ToString());
                return Ok("The employee is set to active status.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Sets an employee's status to quit.
        /// </summary>
        /// <param name="id">The ID of the employee whose status is to be updated.</param>
        /// <returns>A success message if the employee's status is updated successfully.</returns>
        [HttpPatch("{id}/status/quit")] // PATCH: /api/Employees/{id}/status/quit
        public async Task<ActionResult<string>> SetEmployeeQuitStatus(string id)
        {
            try
            {
                await _serviceManager.EmployeeService.SetEmployeeStatusAsync(id, EmployeeStatus.Quit.ToString());
                return Ok("The employee is set to quit status.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates an employee's password.
        /// </summary>
        /// <param name="updatePasswordDTO">The current and new password data.</param>
        /// <returns>A success message if the password is updated successfully.</returns>
        [HttpPatch("password/{id}")] // PATCH: /api/Employees/password
        public async Task<ActionResult<string>> UpdateEmployeePassword([FromRoute(Name = "id")] string id, [FromBody] UpdatePasswordRequest updatePasswordDTO)
        {
            //var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employeeId = id;
            try
            {
                await _serviceManager.EmployeeService.UpdateEmployeePasswordAsync(employeeId, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);
                return Ok("The password is updated successfully.");
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
        /// Adds an address for an employee.
        /// </summary>
        /// <param name="employeeAddress">The employee address data.</param>
        /// <returns>A success message if the address is added successfully.</returns>
        [HttpPost("address/{id}")] // POST: /api/Employees/address
        public async Task<ActionResult<string>> PostEmployeeAddress([FromRoute(Name = "id")] string id, [FromBody] EmployeeAddress employeeAddress)
        {
            // var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            employeeAddress.EmployeeId = id;

            try
            {
                await _serviceManager.EmployeeService.AddEmployeeAddressAsync(employeeAddress);
                return Ok("The employee's address is added successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
