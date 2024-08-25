using AutoMapper;
using LibraryAPI.Entities.DTOs.BookCopyDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class BookCopyManager : IBookCopyService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public BookCopyManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookCopyResponse>> GetAllBookCopiesAsync(bool trackChanges)
        {
            var bookCopies = await _repositoryManager.BookCopyRepository.GetAllBookCopiesAsync(trackChanges);
            return _mapper.Map<IEnumerable<BookCopyResponse>>(bookCopies);
        }

        public async Task<IEnumerable<BookCopyResponse>> GetAllActiveBookCopiesAsync(bool trackChanges)
        {
            var bookCopies = await _repositoryManager.BookCopyRepository.GetAllActiveBookCopiesAsync(Status.Active.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookCopyResponse>>(bookCopies);
        }

        public async Task<IEnumerable<BookCopyResponse>> GetAllInActiveBookCopiesAsync(bool trackChanges)
        {
            var bookCopies = await _repositoryManager.BookCopyRepository.GetAllInActiveBookCopiesAsync(Status.InActive.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookCopyResponse>>(bookCopies);
        }

        public async Task<IEnumerable<BookCopyResponse>> GetAllBorrowedBookCopiesAsync(bool trackChanges)
        {
            var bookCopies = await _repositoryManager.BookCopyRepository.GetAllBorrowedBookCopiesAsync(Status.Borrowed.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookCopyResponse>>(bookCopies);
        }

        public async Task AddLocationOfBookCopiesAsync(List<BookCopyRequest> bookCopyRequests)
        {
            var bookCopyIds = bookCopyRequests.Select(bcr => bcr.BookCopyId).ToList();

            var locationIds = bookCopyRequests.Select(bcr => bcr.LocationId)
                .Where(id => id.HasValue).Select(id => id!.Value)
                .Distinct()
                .ToList();

            var bookCopies = await _repositoryManager.BookCopyRepository
                .FindByCondition(bc => bookCopyIds.Contains(bc.BookCopyId), true)
                .ToListAsync();

            var locations = await _repositoryManager.LocationRepository
                .FindByCondition(l => locationIds.Contains(l.LocationId), false)
                .ToDictionaryAsync(l => l.LocationId);

            var booksInLocation = await _repositoryManager.BookCopyRepository
                .FindByCondition(bc => locationIds.Contains(bc.LocationId!.Value) && bc.BookCopyStatus == Status.Active.ToString(), false)
                .GroupBy(bc => bc.LocationId)
                .ToDictionaryAsync(g => g.Key!.Value, g => g.Count());

            foreach (var bookCopyRequest in bookCopyRequests)
            {
                var bookCopy = bookCopies.FirstOrDefault(bc => bc.BookCopyId == bookCopyRequest.BookCopyId);
                if (bookCopy == null)
                    throw new KeyNotFoundException($"The book copy with ID {bookCopyRequest.BookCopyId} was not found!");

                if (!locations.ContainsKey(bookCopyRequest.LocationId!.Value))
                    throw new KeyNotFoundException($"The location with ID {bookCopyRequest.LocationId} does not exist!");

                if (!booksInLocation.TryGetValue(bookCopyRequest.LocationId.Value, out int currentCount))
                    currentCount = 0;

                if (currentCount >= 50)
                    throw new InvalidOperationException($"The shelf at location ID {bookCopyRequest.LocationId} already has 50 books. No more books can be added.");

                bookCopy.LocationId = bookCopyRequest.LocationId;
                booksInLocation[bookCopyRequest.LocationId.Value] = currentCount + 1;
            }

            await _repositoryManager.SaveAsync();
        }
    }
}
