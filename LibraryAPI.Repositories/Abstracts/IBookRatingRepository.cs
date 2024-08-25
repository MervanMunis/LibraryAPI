using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IBookRatingRepository : IRepositoryBase<BookRating>
    {
        Task<IEnumerable<BookRating>> GetAllBookRatingsAsync(bool trackChanges);
        Task<BookRating> GetBookRatingByIdAsync(long bookId, string memberId, bool trackChanges);
    }
}
