using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    internal class PublisherRepository : RepositoryBase<Publisher>, IPublisherRepository
    {
        public PublisherRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync(bool trackChanges) =>
            await FindAll(trackChanges).Include(b => b.Books).Include(pa => pa.Addresses).ToListAsync();
        

        public async Task<Publisher> GetPublisherByIdAsync(long publisherId, bool trackChanges) =>
            await FindByCondition(b => b.PublisherId.Equals(publisherId), trackChanges)
            .Include(b => b.Books)
            .Include(pa => pa.Addresses)
            .SingleOrDefaultAsync();
    }
}
