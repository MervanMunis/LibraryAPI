using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class AuthorBookRepository : RepositoryBase<AuthorBook>, IAuthorBookRepository
    {
        public AuthorBookRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<AuthorBook>> GetAllAuthorBooksAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<AuthorBook> GetAuthorBookByIdAsync(long authorId, long bookId, bool trackChanges) =>
            await FindByCondition(ab => ab.AuthorsId == authorId && ab.BooksId == bookId, trackChanges).SingleOrDefaultAsync();
    }
}
