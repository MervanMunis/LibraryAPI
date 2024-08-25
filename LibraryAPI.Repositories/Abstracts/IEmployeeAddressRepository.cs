using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IEmployeeAddressRepository : IRepositoryBase<EmployeeAddress>
    {
        Task<IEnumerable<EmployeeAddress>> GetAllEmployeeAddressesAsync(bool trackChanges);
        Task<EmployeeAddress> GetEmployeeAddressByIdAsync(int id, bool trackChanges);
    }
}
