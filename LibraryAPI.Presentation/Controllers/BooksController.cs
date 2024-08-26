using LibraryAPI.Entities.DTOs.BookCopyDTO;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.BookRatingDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Services.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BooksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _serviceManager.BookService.GetAllBooksAsync(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all active books.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveBooks()
        {
            try
            {
                var books = await _serviceManager.BookService.GetAllActiveBooksAsync(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all inactive books.
        /// </summary>
        [HttpGet("inactive")]
        public async Task<IActionResult> GetAllInActiveBooks()
        {
            try
            {
                var books = await _serviceManager.BookService.GetAllInActiveBooksAsync(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all banned books.
        /// </summary>
        [HttpGet("banned")]
        public async Task<IActionResult> GetAllBannedBooks()
        {
            try
            {
                var books = await _serviceManager.BookService.GetAllBannedBooksAsync(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all active book copies.
        /// </summary>
        [HttpGet("bookCopy/active")]
        public async Task<IActionResult> GetAllActiveBookCopies()
        {
            try
            {
                var bookCopies = await _serviceManager.BookCopyService.GetAllActiveBookCopiesAsync(false);
                return Ok(bookCopies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all inactive book copies.
        /// </summary>
        [HttpGet("bookCopy/inactive")]
        public async Task<IActionResult> GetAllInActiveBookCopies()
        {
            try
            {
                var bookCopies = await _serviceManager.BookCopyService
                    .GetAllInActiveBookCopiesAsync(false);

                return Ok(bookCopies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all borrowed book copies.
        /// </summary>
        [HttpGet("bookCopy/borrowed")]
        public async Task<IActionResult> GetAllBorrowedBookCopies()
        {
            try
            {
                var bookCopies = await _serviceManager.BookCopyService
                    .GetAllBorrowedBookCopiesAsync(false);
                return Ok(bookCopies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific book by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(long id)
        {
            try
            {
                var book = await _serviceManager.BookService.GetBookByIdAsync(id, false);
                if (book == null)
                    return NotFound("Book not found.");

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] BookRequest bookRequest)
        {
            if (bookRequest == null)
                return BadRequest("Book request cannot be null.");

            try
            {
                await _serviceManager.BookService.AddBookAsync(bookRequest);
                return CreatedAtAction(nameof(GetBook), new { id = bookRequest.ISBN }, "The book is added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Adds location information to book copies.
        /// </summary>
        [HttpPost("bookCopies/locations")]
        public async Task<IActionResult> AddLocationOfBookCopies([FromBody] List<BookCopyRequest> bookCopyRequests)
        {
            if (bookCopyRequests == null || !bookCopyRequests.Any())
                return BadRequest("Book copy requests cannot be null or empty.");

            try
            {
                await _serviceManager.BookCopyService.AddLocationOfBookCopiesAsync(bookCopyRequests);
                return Ok("Locations of book copies have been added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, [FromBody] BookRequest bookRequest)
        {
            if (bookRequest == null)
                return BadRequest("Book request cannot be null.");

            try
            {
                await _serviceManager.BookService.UpdateBookAsync(id, bookRequest);
                return Ok("The book is updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sets the status of a book to inactive.
        /// </summary>
        [HttpPatch("{id}/inactive")]
        public async Task<IActionResult> InactiveBook(long id)
        {
            try
            {
                await _serviceManager.BookService.SetBookStatusAsync(id, Status.InActive.ToString());
                return Ok("The status of the book is now inactive.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sets the status of a book to active.
        /// </summary>
        [HttpPatch("{id}/active")]
        public async Task<IActionResult> ActiveBook(long id)
        {
            try
            {
                await _serviceManager.BookService.SetBookStatusAsync(id, Status.Active.ToString());
                return Ok("The status of the book is now active.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sets the status of a book to banned.
        /// </summary>
        [HttpPatch("{id}/banned")]
        public async Task<IActionResult> BannedBook(long id)
        {
            try
            {
                await _serviceManager.BookService.SetBookStatusAsync(id, Status.Banned.ToString());
                return Ok("The status of the book is now banned.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the cover image of a book.
        /// </summary>
        [HttpPatch("{id}/image")]
        public async Task<IActionResult> UpdateBookImage(long id, IFormFile coverImage)
        {
            if (coverImage == null)
                return BadRequest("Image file is required.");

            try
            {
                await _serviceManager.BookService.UpdateBookImageAsync(id, coverImage);
                return Ok("The book's image has been updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the image of a book by book ID.
        /// </summary>
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetBookImage(long id)
        {
            try
            {
                var image = await _serviceManager.BookService.GetBookImageAsync(id);
                if (image == null)
                    return NotFound("Image not found.");

                return File(image, "image/jpeg"); // Adjust MIME type as necessary.
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the rating of a book.
        /// </summary>
        [HttpPut("{id}/rating")]
        public async Task<IActionResult> UpdateBookRating(long id, [FromBody] BookRatingRequest ratingRequest)
        {
            if (ratingRequest == null || string.IsNullOrEmpty(ratingRequest.MemberId))
                return BadRequest("Invalid rating request.");

            try
            {
                await _serviceManager.BookService.UpdateBookRatingAsync(id, ratingRequest.GivenRating, ratingRequest.MemberId!);
                return Ok("The book rating has been updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the number of copies of a book.
        /// </summary>
        [HttpPut("{id}/copies")]
        public async Task<IActionResult> UpdateBookCopies(long id, [FromBody] short change)
        {
            try
            {
                await _serviceManager.BookService.UpdateBookCopiesAsync(id, change);
                return Ok("The book copies have been updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
