using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges);
        Task<Employee> GetEmployeeByIdAsync(string employeeId, bool trackChanges);
    }
}
