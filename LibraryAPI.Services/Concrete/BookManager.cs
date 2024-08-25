using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class BookManager :IBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public BookManager(IRepositoryManager repositoryManager, IMapper mapper, IFileService fileService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository.GetAllBooksAsync(trackChanges);
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<IEnumerable<BookResponse>> GetAllActiveBooksAsync(bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository.GetAllActiveBooksAsync(Status.Active.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<IEnumerable<BookResponse>> GetAllInActiveBooksAsync(bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository.GetAllInActiveBooksAsync(Status.InActive.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<IEnumerable<BookResponse>> GetAllBannedBooksAsync(bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository.GetAllBannedBooksAsync(Status.Banned.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<BookResponse> GetBookByIdAsync(long id, bool trackChanges)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, trackChanges);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            if (book.BookStatus != Status.Active.ToString())
                throw new InvalidOperationException("The book is not active");

            return _mapper.Map<BookResponse>(book);
        }

        public async Task AddBookAsync(BookRequest bookRequest)
        {
            var existingBook = await _repositoryManager.BookRepository.FindByCondition(b => b.ISBN == bookRequest.ISBN, false).FirstOrDefaultAsync();
            if (existingBook != null)
                throw new InvalidOperationException("The book with the specified ISBN is already in the database!");

            var newBook = _mapper.Map<Book>(bookRequest);
            newBook.BookStatus = Status.Active.ToString();

            await _repositoryManager.BookRepository.CreateAsync(newBook);
            await _repositoryManager.SaveAsync();

            var bookCopies = Enumerable.Range(0, bookRequest.CopyCount).Select(_ => new BookCopy
            {
                BookId = newBook.BookId,
                BookCopyStatus = Status.Active.ToString()
            }).ToList();

            await _repositoryManager.BookCopyRepository.CreateAsync(bookCopies);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateBookAsync(long id, BookRequest bookRequest)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            if (book.ISBN != bookRequest.ISBN)
            {
                var existingBook = await _repositoryManager.BookRepository.FindByCondition(b => b.ISBN == bookRequest.ISBN, false).FirstOrDefaultAsync();
                if (existingBook != null)
                    throw new InvalidOperationException("The book with the specified ISBN already exists!");
            }

            _mapper.Map(bookRequest, book);
            _repositoryManager.BookRepository.Update(book);

            await UpdateBookCrossTablesAsync(id, bookRequest);
            await _repositoryManager.SaveAsync();
        }

        public async Task SetBookStatusAsync(long id, string status)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            book.BookStatus = status;
            _repositoryManager.BookRepository.Update(book);

            var bookCopies = await _repositoryManager.BookCopyRepository
                .FindByCondition(bc => bc.BookId == id, true)
                .ToListAsync();

            foreach (var copy in bookCopies)
            {
                copy.BookCopyStatus = status;
                _repositoryManager.BookCopyRepository.Update(copy);
            }

            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateBookImageAsync(long id, IFormFile coverImage)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            var filePath = await _fileService.SaveFileAsync(coverImage, "BookImages");
            book.CoverFileName = filePath;

            _repositoryManager.BookRepository.Update(book);
            await _repositoryManager.SaveAsync();
        }

        public async Task<byte[]> GetBookImageAsync(long id)
        {
            // Check if the book exists
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, false);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            // Get the image from the file service
            return await _fileService.GetImageByBookIdAsync(id);
        }

        public async Task UpdateBookRatingAsync(long id, float rating, string memberId)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5!");

            // Look for an existing rating by this member for this book
            var existingRating = await _repositoryManager.BookRatingRepository
                .FindByCondition(br => br.BookId == id && br.MemberId == memberId, true)
                .FirstOrDefaultAsync();

            if (existingRating != null)
            {
                // Update the existing rating
                existingRating.GivenRating = rating;
                _repositoryManager.BookRatingRepository.Update(existingRating);
            }
            else
            {
                // Create a new rating if none exists
                var bookRating = new BookRating
                {
                    BookId = id,
                    MemberId = memberId,
                    GivenRating = rating
                };
                await _repositoryManager.BookRatingRepository.CreateAsync(bookRating);
            }

            await _repositoryManager.SaveAsync();
            await RecalculateBookRatingAsync(id);
        }


        public async Task UpdateBookCopiesAsync(long id, short change)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            var activeCopiesCount = await _repositoryManager.BookCopyRepository.FindByCondition(bc => bc.BookId == id && bc.BookCopyStatus == Status.Active.ToString(), false).CountAsync();

            if (activeCopiesCount + change < 0)
                throw new InvalidOperationException("Not enough copies available");

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
                var copiesToRemove = await _repositoryManager.BookCopyRepository.FindByCondition(bc => bc.BookId == id && bc.BookCopyStatus == Status.Active.ToString(), true)
                    .Take(Math.Abs(change)).ToListAsync();

                foreach (var copy in copiesToRemove)
                {
                    copy.BookCopyStatus = Status.InActive.ToString();
                    _repositoryManager.BookCopyRepository.Update(copy);
                }
            }

            await _repositoryManager.SaveAsync();
        }

        private async Task UpdateBookCrossTablesAsync(long id, BookRequest bookRequest)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(id, true);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            // Update AuthorBooks relationship
            var existingAuthorBooks = await _repositoryManager.AuthorBookRepository
                .FindByCondition(ab => ab.BooksId == id, true)
                .ToListAsync();

            var newAuthorBooks = bookRequest.AuthorIds
                .Where(authorId => !existingAuthorBooks.Any(ab => ab.AuthorsId == authorId))
                .Select(authorId => new AuthorBook { AuthorsId = authorId, BooksId = book.BookId })
                .ToList();

            var authorBooksToRemove = existingAuthorBooks
                .Where(ab => !bookRequest.AuthorIds.Contains(ab.AuthorsId!.Value))
                .ToList();

            if (authorBooksToRemove.Any())
            {
                _repositoryManager.AuthorBookRepository.Delete(authorBooksToRemove);
            }

            if (newAuthorBooks.Any())
            {
                await _repositoryManager.AuthorBookRepository.CreateAsync(newAuthorBooks);
            }

            // Update BookLanguages relationship
            var existingBookLanguages = await _repositoryManager.BookLanguageRepository
                .FindByCondition(bl => bl.BooksId == id, true)
                .ToListAsync();

            var newBookLanguages = bookRequest.LanguageIds
                .Where(languageId => !existingBookLanguages.Any(bl => bl.LanguagesId == languageId))
                .Select(languageId => new BookLanguage { LanguagesId = languageId, BooksId = book.BookId })
                .ToList();

            var bookLanguagesToRemove = existingBookLanguages
                .Where(bl => !bookRequest.LanguageIds.Contains(bl.LanguagesId!.Value))
                .ToList();

            if (bookLanguagesToRemove.Any())
            {
                _repositoryManager.BookLanguageRepository.Delete(bookLanguagesToRemove);
            }

            if (newBookLanguages.Any())
            {
                await _repositoryManager.BookLanguageRepository.CreateAsync(newBookLanguages);
            }

            // Update BookSubCategories relationship
            var existingBookSubCategories = await _repositoryManager.BookSubCategoryRepository
                .FindByCondition(bsc => bsc.BooksId == id, true)
                .ToListAsync();

            var newBookSubCategories = bookRequest.SubCategoryIds
                .Where(subCategoryId => !existingBookSubCategories.Any(bsc => bsc.SubCategoriesId == subCategoryId))
                .Select(subCategoryId => new BookSubCategory { SubCategoriesId = subCategoryId, BooksId = book.BookId })
                .ToList();

            var bookSubCategoriesToRemove = existingBookSubCategories
                .Where(bsc => !bookRequest.SubCategoryIds.Contains(bsc.SubCategoriesId!.Value))
                .ToList();

            if (bookSubCategoriesToRemove.Any())
            {
                _repositoryManager.BookSubCategoryRepository.Delete(bookSubCategoriesToRemove);
            }

            if (newBookSubCategories.Any())
            {
                await _repositoryManager.BookSubCategoryRepository.CreateAsync(newBookSubCategories);
            }

            await _repositoryManager.SaveAsync();
        }

        private async Task RecalculateBookRatingAsync(long bookId)
        {
            var averageRating = await _repositoryManager.BookRatingRepository.FindByCondition(br => br.BookId == bookId, false)
                .AverageAsync(br => br.GivenRating);

            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(bookId, true);
            if (book != null)
            {
                book.Rating = averageRating;
                _repositoryManager.BookRepository.Update(book);
                await _repositoryManager.SaveAsync();
            }
        }
    }
}
