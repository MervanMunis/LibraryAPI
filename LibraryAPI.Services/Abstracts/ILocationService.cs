using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LocationDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationResponse>> GetAllLocationsAsync(bool trackChanges);
        Task<LocationResponse> GetLocationByIdAsync(int id, bool trackChanges);
        Task AddLocationAsync(LocationRequest locationRequest);
        Task UpdateLocationAsync(int id, LocationRequest locationRequest);
        Task SetLocationStatusAsync(int id, string status);
        Task<IEnumerable<BookResponse>> GetBooksBySectionCodeAsync(string sectionCode, bool trackChanges);
        Task<IEnumerable<BookResponse>> GetBooksByAisleCodeAsync(string aisleCode, bool trackChanges);
        Task<IEnumerable<BookResponse>> GetBooksByShelfNumberAsync(string shelfNumber, bool trackChanges);
    }
}
