using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class EmployeeAddressRepository : RepositoryBase<EmployeeAddress>, IEmployeeAddressRepository
    {
        public EmployeeAddressRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeAddress>> GetAllEmployeeAddressesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();

        public async Task<EmployeeAddress> GetEmployeeAddressByIdAsync(int id, bool trackChanges)
            => await FindByCondition(b => b.EmployeeAddressId.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
