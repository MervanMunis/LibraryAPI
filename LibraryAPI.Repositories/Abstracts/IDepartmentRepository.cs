using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IDepartmentRepository : IRepositoryBase<Department>
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges);
        Task<Department> GetDepartmentByIdAsync(short departmentId, bool trackChanges);
    }
}
