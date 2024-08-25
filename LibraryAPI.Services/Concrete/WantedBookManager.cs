using AutoMapper;
using LibraryAPI.Entities.DTOs.WantedBookDTO;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;

namespace LibraryAPI.Services.Concrete
{
    public class WantedBookManager : IWantedBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public WantedBookManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WantedBookResponse>> GetAllWantedBooksAsync(bool trackChanges)
        {
            var wantedBooks = await _repositoryManager.WantedBookRepository
                .GetAllWantedBooksAsync(trackChanges);

            var wantedBookResponses = _mapper.Map<IEnumerable<WantedBookResponse>>(wantedBooks);
            return wantedBookResponses;
        }

        public async Task<WantedBookResponse> GetWantedBookByIdAsync(int id, bool trackChanges)
        {
            var wantedBook = await _repositoryManager.WantedBookRepository
                .GetWantedBookByIdAsync(id, trackChanges);

            if (wantedBook == null)
                throw new KeyNotFoundException("Wanted book not found");

            var wantedBookResponse = _mapper.Map<WantedBookResponse>(wantedBook);
            return wantedBookResponse;
        }

        public async Task<string> AddWantedBookAsync(WantedBookRequest wantedBookRequest)
        {
            var wantedBookEntity = _mapper.Map<WantedBook>(wantedBookRequest);

            await _repositoryManager.WantedBookRepository.CreateAsync(wantedBookEntity);
            await _repositoryManager.SaveAsync();

            return "Wanted book successfully created!";
        }

        public async Task<bool> UpdateWantedBookAsync(int id, WantedBookRequest wantedBookRequest)
        {
            var wantedBookEntity = await _repositoryManager.WantedBookRepository
                .GetWantedBookByIdAsync(id, trackChanges: true);

            if (wantedBookEntity == null)
                throw new KeyNotFoundException("Wanted book not found");

            _mapper.Map(wantedBookRequest, wantedBookEntity);

            _repositoryManager.WantedBookRepository.Update(wantedBookEntity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteWantedBookAsync(int id)
        {
            var wantedBook = await _repositoryManager.WantedBookRepository
                .GetWantedBookByIdAsync(id, trackChanges: false);

            if (wantedBook == null)
                throw new KeyNotFoundException("Wanted book not found");

            _repositoryManager.WantedBookRepository.Delete(wantedBook);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
