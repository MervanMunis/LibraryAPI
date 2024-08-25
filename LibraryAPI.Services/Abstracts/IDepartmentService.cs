using LibraryAPI.Entities.DTOs.DepartmentDTO;
using LibraryAPI.Entities.DTOs.EmployeeDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(bool trackChanges);

        Task<DepartmentResponse> GetDepartmentByIdAsync(short id, bool trackChanges);

        Task AddDepartmentAsync(DepartmentRequest departmentRequest);

        Task UpdateDepartmentAsync(short id, DepartmentRequest departmentRequest);

        Task<IEnumerable<EmployeeDepartmentResponse>> GetEmployeesByDepartmentIdAsync(short id, bool trackChanges);

        Task DeleteDepartmentAsync(short id);
    }
}
