using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.PublisherAddressDTO;
using LibraryAPI.Entities.DTOs.PublisherDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherResponse>> GetAllPublishersAsync(bool trackChanges);
        Task<PublisherResponse> GetPublisherByIdAsync(long id, bool trackChanges);
        Task<string> AddPublisherAsync(PublisherRequest publisherRequest);
        Task<bool> UpdatePublisherAsync(long id, PublisherRequest publisherRequest);
        Task<bool> SetPublisherStatusAsync(long id, string status);
        Task<IEnumerable<CategoryBookResponse>> GetBooksByPublisherIdAsync(long publisherId, bool trackChanges);
        Task<PublisherAddressResponse> GetPublisherAddressAsync(int id, bool trackChanges);
        Task<string> AddOrUpdatePublisherAddressAsync(long publisherId, PublisherAddressRequest addressRequest);
    }
}
