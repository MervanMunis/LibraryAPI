using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class PublisherAddressRepository : RepositoryBase<PublisherAddress>, IPublisherAddressRepository
    {
        public PublisherAddressRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PublisherAddress>> GetAllPublisherAddressesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();


        public async Task<PublisherAddress> GetPublisherAddressByIdAsync(int id, bool trackChanges)
            => await FindByCondition(b => b.PublisherAddressId.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
