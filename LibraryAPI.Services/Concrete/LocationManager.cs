using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.LocationDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class LocationManager : ILocationService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public LocationManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationResponse>> GetAllLocationsAsync(bool trackChanges)
        {
            var locations = await _repositoryManager.LocationRepository
                .FindByCondition(l => l.LocationStatus == Status.Active.ToString(), trackChanges)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LocationResponse>>(locations);
        }

        public async Task<LocationResponse> GetLocationByIdAsync(int id, bool trackChanges)
        {
            var location = await _repositoryManager.LocationRepository
                .FindByCondition(l => l.LocationId == id && l.LocationStatus == Status.Active.ToString(), trackChanges)
                .FirstOrDefaultAsync();

            if (location == null)
                throw new KeyNotFoundException("Location not found");

            return _mapper.Map<LocationResponse>(location);
        }

        public async Task AddLocationAsync(LocationRequest locationRequest)
        {
            var location = _mapper.Map<Location>(locationRequest);
            await _repositoryManager.LocationRepository.CreateAsync(location);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateLocationAsync(int id, LocationRequest locationRequest)
        {
            var location = await _repositoryManager.LocationRepository.GetLocationByIdAsync(id, true);
            if (location == null)
                throw new KeyNotFoundException("Location not found");

            _mapper.Map(locationRequest, location);
            _repositoryManager.LocationRepository.Update(location);

            await _repositoryManager.SaveAsync();
        }

        public async Task SetLocationStatusAsync(int id, string status)
        {
            var location = await _repositoryManager.LocationRepository.GetLocationByIdAsync(id, true);
            if (location == null)
                throw new KeyNotFoundException("Location not found");

            if (status == Status.InActive.ToString())
            {
                var booksInLocation = await _repositoryManager.BookCopyRepository
                    .FindByCondition(book => book.LocationId == id && book.BookCopyStatus == Status.Active.ToString(), false)
                    .ToListAsync();

                if (booksInLocation.Any())
                    throw new InvalidOperationException("Location cannot be set to inactive as there are active books in this location.");
            }

            location.LocationStatus = status;
            _repositoryManager.LocationRepository.Update(location);

            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<BookResponse>> GetBooksBySectionCodeAsync(string sectionCode, bool trackChanges)
        {
            var books = await _repositoryManager.BookCopyRepository
                .FindByCondition(b => b.Location!.SectionCode == sectionCode && b.BookCopyStatus == Status.Active.ToString(), trackChanges)
                .Include(b => b.Book)
                .ToListAsync();

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this section code");

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByAisleCodeAsync(string aisleCode, bool trackChanges)
        {
            var books = await _repositoryManager.BookCopyRepository
                .FindByCondition(b => b.Location!.AisleCode == aisleCode && b.BookCopyStatus == Status.Active.ToString(), trackChanges)
                .Include(b => b.Book)
                .ToListAsync();

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this aisle code");

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<IEnumerable<BookResponse>> GetBooksByShelfNumberAsync(string shelfNumber, bool trackChanges)
        {
            var books = await _repositoryManager.BookCopyRepository
                .FindByCondition(b => b.Location!.ShelfNumber == shelfNumber && b.BookCopyStatus == Status.Active.ToString(), trackChanges)
                .Include(b => b.Book)
                .ToListAsync();

            if (!books.Any())
                throw new KeyNotFoundException("No books found for this shelf number");

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }
    }
}
