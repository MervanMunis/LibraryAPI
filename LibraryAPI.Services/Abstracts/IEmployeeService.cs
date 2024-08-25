using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Services.Abstracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync(bool trackChanges);

        Task<EmployeeResponse> GetEmployeeByIdNumberAsync(string idNumber, bool trackChanges);

        Task AddEmployeeAsync(EmployeeRequest employeeRequest);

        Task UpdateEmployeeAsync(string id, EmployeeRequest employeeRequest);

        Task SetEmployeeStatusAsync(string id, string status);

        Task AddEmployeeAddressAsync(EmployeeAddress employeeAddress);

        Task UpdateEmployeePasswordAsync(string id, string currentPassword, string newPassword);
    }
}
