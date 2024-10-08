﻿using LibraryAPI.Entities.DTOs.BookCopyDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface IBookCopyService
    {
        Task<IEnumerable<BookCopyResponse>> GetAllBookCopiesAsync(bool trackChanges);
        Task<IEnumerable<BookCopyResponse>> GetAllActiveBookCopiesAsync(bool trackChanges);
        Task<IEnumerable<BookCopyResponse>> GetAllInActiveBookCopiesAsync(bool trackChanges);
        Task<IEnumerable<BookCopyResponse>> GetAllBorrowedBookCopiesAsync(bool trackChanges);
        Task<string> AddLocationOfBookCopiesAsync(List<BookCopyRequest> bookCopyRequests);
        Task UpdateBookCopiesAsync(long id, short change);
    }
}
