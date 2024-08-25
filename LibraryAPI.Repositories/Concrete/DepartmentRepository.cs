using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Department> GetDepartmentByIdAsync(short departmentId, bool trackChanges) =>
            await FindByCondition(d => d.DepartmentId.Equals(departmentId), trackChanges).SingleOrDefaultAsync();
    }
}
