using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface ILocationRepository : IRepositoryBase<Location>
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync(bool trackChanges);
        Task<Location> GetLocationByIdAsync(int locationId, bool trackChanges);
    }
}
