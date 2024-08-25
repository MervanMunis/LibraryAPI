using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Location> GetLocationByIdAsync(int locationId, bool trackChanges) =>
            await FindByCondition(l => l.LocationId.Equals(locationId), trackChanges).SingleOrDefaultAsync();
    }
}
