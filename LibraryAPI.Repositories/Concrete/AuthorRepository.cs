using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Author>> GetAllActiveAuthorsAsync(string active, bool trackChanges)
            => await FindByCondition(a => a.AuthroStatus.Equals(active), trackChanges)
            .ToListAsync();


        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<IEnumerable<Author>> GetAllBannedAuthorsAsync(string banned, bool trackChanges)
            => await FindByCondition(a => a.AuthroStatus.Equals(banned), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Author>> GetAllInActiveAuthorsAsync(string inActive, bool trackChanges)
        => await FindByCondition(a => a.AuthroStatus.Equals(inActive), trackChanges)
            .ToListAsync();

        public async Task<Author> GetAuthorByIdAsync(long authorId, bool trackChanges) =>
                    await FindByCondition(a => a.AuthorId.Equals(authorId), trackChanges).SingleOrDefaultAsync();
    }
}
