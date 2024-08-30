using LibraryAPI.Entities.DTOs.DepartmentDTO;
using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DepartmentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>A list of all departments.</returns>
        [HttpGet] // GET: api/Departments
        [Authorize(Roles = "Librarian,DepartmentHead,HeadOfLibrary")]
        public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetDepartments()
        {
            var departments = await _serviceManager.DepartmentService.GetAllDepartmentsAsync(false);
            return Ok(departments);
        }

        /// <summary>
        /// Retrieves a specific department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to retrieve.</param>
        /// <returns>The details of the department.</returns>
        [HttpGet("{id}")] // GET: api/Departments/5
        [Authorize(Roles = "Librarian,DepartmentHead,HeadOfLibrary")]
        public async Task<ActionResult<DepartmentResponse>> GetDepartment(short id)
        {
            try
            {
                var department = await _serviceManager.DepartmentService.GetDepartmentByIdAsync(id, false);
                return Ok(department);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new department.
        /// </summary>
        /// <param name="departmentRequest">The request body containing details of the department to add.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPost] // POST: api/Departments
        [Authorize(Roles = "HeadOfLibrary")]
        public async Task<ActionResult<string>> PostDepartment([FromBody] DepartmentRequest departmentRequest)
        {
            try
            {
                await _serviceManager.DepartmentService.AddDepartmentAsync(departmentRequest);
                return Ok("The department is successfully created.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="id">The ID of the department to update.</param>
        /// <param name="departmentRequest">The request body containing updated details of the department.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpPut("{id}")] // PUT: api/Departments/5
        [Authorize(Roles = "HeadOfLibrary")]
        public async Task<ActionResult<string>> PutDepartment(short id, [FromBody] DepartmentRequest departmentRequest)
        {
            try
            {
                await _serviceManager.DepartmentService.UpdateDepartmentAsync(id, departmentRequest);
                return Ok("The department is updated successfully.");
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
        /// Retrieves all employees associated with a specific department.
        /// </summary>
        /// <param name="id">The ID of the department whose employees to retrieve.</param>
        /// <returns>A list of employees in the specified department.</returns>
        [HttpGet("{id}/employees")]
        [Authorize(Roles = "Librarian,DepartmentHead,HeadOfLibrary")]
        public async Task<ActionResult<IEnumerable<EmployeeDepartmentResponse>>> GetEmployeesByDepartmentId(short id)
        {
            try
            {
                var employees = await _serviceManager.DepartmentService.GetEmployeesByDepartmentIdAsync(id, false);
                return Ok(employees);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a department.
        /// </summary>
        /// <param name="id">The ID of the department to delete.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        [HttpDelete("{id}")] // DELETE: api/Departments/5
        [Authorize(Roles = "HeadOfLibrary")]
        public async Task<ActionResult<string>> DeleteDepartment(short id)
        {
            try
            {
                await _serviceManager.DepartmentService.DeleteDepartmentAsync(id);
                return Ok("The department is deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
