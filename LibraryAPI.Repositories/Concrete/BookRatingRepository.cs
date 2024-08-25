using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class BookRatingRepository : RepositoryBase<BookRating>, IBookRatingRepository
    {
        public BookRatingRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<BookRating>> GetAllBookRatingsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<BookRating> GetBookRatingByIdAsync(long bookId, string memberId, bool trackChanges) =>
            await FindByCondition(br => br.BookId == bookId && br.MemberId == memberId, trackChanges).SingleOrDefaultAsync();
    }
}
