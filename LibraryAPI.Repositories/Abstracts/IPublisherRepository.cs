using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IPublisherRepository : IRepositoryBase<Publisher>
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync(bool trackChanges);
        Task<Publisher> GetPublisherByIdAsync(long publisherId, bool trackChanges);
    }
}
