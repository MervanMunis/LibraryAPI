using LibraryAPI.Entities.DTOs.BookCopyDTO;
using LibraryAPI.Entities.DTOs.BookDTO;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Services.Abstracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponse>> GetAllBooksAsync(bool trackChanges);
        Task<IEnumerable<BookResponse>> GetAllActiveBooksAsync(bool trackChanges);
        Task<IEnumerable<BookResponse>> GetAllInActiveBooksAsync(bool trackChanges);
        Task<IEnumerable<BookResponse>> GetAllBannedBooksAsync(bool trackChanges);
        Task<BookResponse> GetBookByIdAsync(long id, bool trackChanges);
        Task AddBookAsync(BookRequest bookRequest);
        Task UpdateBookAsync(long id, BookRequest bookRequest);
        Task SetBookStatusAsync(long id, string status);
        Task UpdateBookImageAsync(long id, IFormFile coverImage);
        Task<byte[]> GetBookImageAsync(long id);
        Task UpdateBookRatingAsync(long id, float rating, string memberId);
        Task UpdateBookCopiesAsync(long id, short change);
    }
}
