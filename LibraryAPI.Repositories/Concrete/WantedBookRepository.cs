using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class WantedBookRepository : RepositoryBase<WantedBook>, IWantedBookRepository
    {
        public WantedBookRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<WantedBook>> GetAllWantedBooksAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<WantedBook> GetWantedBookByIdAsync(int wantedBookId, bool trackChanges) =>
            await FindByCondition(wb => wb.WantedBookId.Equals(wantedBookId), trackChanges).SingleOrDefaultAsync();
    }
}
