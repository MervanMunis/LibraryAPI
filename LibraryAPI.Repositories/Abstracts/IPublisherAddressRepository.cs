using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IPublisherAddressRepository : IRepositoryBase<PublisherAddress>
    {
        Task<IEnumerable<PublisherAddress>> GetAllPublisherAddressesAsync(bool trackChanges);
        Task<PublisherAddress> GetPublisherAddressByIdAsync(int id, bool trackChanges);
    }
}
