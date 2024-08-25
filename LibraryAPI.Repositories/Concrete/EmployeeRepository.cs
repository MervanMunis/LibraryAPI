using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Employee> GetEmployeeByIdAsync(string employeeId, bool trackChanges) =>
            await FindByCondition(e => e.EmployeeId.Equals(employeeId), trackChanges).SingleOrDefaultAsync();
    }
}
