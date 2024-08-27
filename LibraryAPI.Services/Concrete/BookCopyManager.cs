using AutoMapper;
using LibraryAPI.Entities.DTOs.BookCopyDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using LibraryAPI.Services.Exceptions;
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

        public async Task<string> AddLocationOfBookCopiesAsync(List<BookCopyRequest> bookCopyRequests)
        {
            var bookCopyIds = bookCopyRequests.Select(bcr => bcr.BookCopyId).ToList();

            var locationIds = bookCopyRequests.Select(bcr => bcr.LocationId)
                .Where(id => id.HasValue).Select(id => id!.Value)
                .Distinct()
                .ToList();

            // Fetch all book copies in one query
            var bookCopies = await _repositoryManager.BookCopyRepository
                .FindByCondition(bc => bookCopyIds.Contains(bc.BookCopyId), true)
                .ToListAsync();

            // Fetch all locations in one query
            var locations = await _repositoryManager.LocationRepository
                .FindByCondition(l => locationIds.Contains(l.LocationId), false)
                .ToDictionaryAsync(l => l.LocationId);

            // Dictionary to track the number of books per location
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

            try
            {
                await _repositoryManager.SaveAsync();
                return "All the book copies were placed on the specified locations successfully.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return $"A concurrency error occurred: {ex.Message}";
            }
        }

        public async Task UpdateBookCopiesAsync(long id, short change)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new NotFoundException("Book not found");

            var activeCopiesCount = await _repositoryManager.BookCopyRepository.FindByCondition(bc => bc.BookId == id && bc.BookCopyStatus == Status.Active.ToString(), false).CountAsync();

            if (activeCopiesCount + change < 0)
                throw new BadRequestException("Not enough copies available");

            if (change > 0)
            {
                var newCopies = Enumerable.Range(0, change).Select(_ => new BookCopy
                {
                    BookId = id,
                    BookCopyStatus = Status.Active.ToString()
                }).ToList();

                await _repositoryManager.BookCopyRepository.CreateAsync(newCopies);
            }
            else if (change < 0)
            {
                var copiesToRemove = await _repositoryManager.BookCopyRepository
                    .FindByCondition(bc => bc.BookId == id && bc.BookCopyStatus == Status.Active.ToString(), true)
                    .Take(Math.Abs(change))
                    .ToListAsync();

                foreach (var copy in copiesToRemove)
                {
                    copy.BookCopyStatus = Status.InActive.ToString();
                    _repositoryManager.BookCopyRepository.Update(copy);
                }
            }

            await _repositoryManager.SaveAsync();
        }
    }
}
