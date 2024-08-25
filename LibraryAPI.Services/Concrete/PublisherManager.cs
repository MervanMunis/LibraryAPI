using AutoMapper;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.PublisherAddressDTO;
using LibraryAPI.Entities.DTOs.PublisherDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;

namespace LibraryAPI.Services.Concrete
{
    public class PublisherManager : IPublisherService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public PublisherManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PublisherResponse>> GetAllPublishersAsync(bool trackChanges)
        {
            var publishers = await _repositoryManager.PublisherRepository
                .GetAllPublishersAsync(trackChanges);

            var publisherResponses = _mapper.Map<IEnumerable<PublisherResponse>>(publishers);

            return publisherResponses;
        }

        public async Task<PublisherResponse> GetPublisherByIdAsync(long id, bool trackChanges)
        {
            var publisher = await _repositoryManager.PublisherRepository
                .GetPublisherByIdAsync(id, trackChanges);

            if (publisher == null)
                throw new KeyNotFoundException("Publisher not found");

            var publisherResponse = _mapper.Map<PublisherResponse>(publisher);

            return publisherResponse;
        }

        public async Task<string> AddPublisherAsync(PublisherRequest publisherRequest)
        {
            var publisherEntity = _mapper.Map<Publisher>(publisherRequest);

            await _repositoryManager.PublisherRepository.CreateAsync(publisherEntity);
            await _repositoryManager.SaveAsync();

            return "Publisher successfully created!";
        }

        public async Task<bool> UpdatePublisherAsync(long id, PublisherRequest publisherRequest)
        {
            var publisherEntity = await _repositoryManager.PublisherRepository
                .GetPublisherByIdAsync(id, trackChanges: true);

            if (publisherEntity == null)
                throw new KeyNotFoundException("Publisher not found");

            _mapper.Map(publisherRequest, publisherEntity);

            _repositoryManager.PublisherRepository.Update(publisherEntity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> SetPublisherStatusAsync(long id, string status)
        {
            var publisherEntity = await _repositoryManager.PublisherRepository
                .GetPublisherByIdAsync(id, trackChanges: true);

            if (publisherEntity == null)
                throw new KeyNotFoundException("Publisher not found");

            if (status == Status.InActive.ToString() &&
                (await _repositoryManager.BookRepository.GetBooksByPublisherIdAsync(id, trackChanges: false)).Any())
            {
                throw new InvalidOperationException("Cannot deactivate publisher with active books.");
            }

            publisherEntity.PublisherStatus = status;
            _repositoryManager.PublisherRepository.Update(publisherEntity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryBookResponse>> GetBooksByPublisherIdAsync(long publisherId, bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository
                .GetBooksByPublisherIdAsync(publisherId, trackChanges);

            return _mapper.Map<IEnumerable<CategoryBookResponse>>(books);
        }

        public async Task<PublisherAddressResponse> GetPublisherAddressAsync(int id, bool trackChanges)
        {
            var publisherAddress = await _repositoryManager.PublisherAddressRepository
                .GetPublisherAddressByIdAsync(id, trackChanges);

            if (publisherAddress == null)
                throw new KeyNotFoundException("Publisher address not found");

            var addressResponse = _mapper.Map<PublisherAddressResponse>(publisherAddress);

            return addressResponse;
        }

        public async Task<string> AddOrUpdatePublisherAddressAsync(long publisherId, PublisherAddressRequest addressRequest)
        {
            var publisherEntity = await _repositoryManager.PublisherRepository
                .GetPublisherByIdAsync(publisherId, trackChanges: false);

            if (publisherEntity == null)
                throw new KeyNotFoundException("Publisher not found");

            var publisherAddress = _mapper.Map<PublisherAddress>(addressRequest);
            publisherAddress.PublisherId = publisherId;

            await _repositoryManager.PublisherAddressRepository.CreateAsync(publisherAddress);
            await _repositoryManager.SaveAsync();

            return "Publisher address successfully added or updated!";
        }
    }
}
