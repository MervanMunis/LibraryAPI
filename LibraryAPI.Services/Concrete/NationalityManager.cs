using AutoMapper;
using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Entities.DTOs.NationalityDTO;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class NationalityManager : INationalityService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public NationalityManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NationalityResponse>> GetAllNationalitiesAsync(bool trackChanges)
        {
            var nationalities = await _repositoryManager.NationalityRepository.GetAllNationalitiesAsync(trackChanges);
            return _mapper.Map<IEnumerable<NationalityResponse>>(nationalities);
        }

        public async Task<NationalityResponse> GetNationalityByIdAsync(short id, bool trackChanges)
        {
            var nationality = await _repositoryManager.NationalityRepository.GetNationalityByIdAsync(id, trackChanges);
            if (nationality == null) throw new KeyNotFoundException("Nationality not found");

            return _mapper.Map<NationalityResponse>(nationality);
        }

        public async Task AddNationalityAsync(NationalityRequest nationalityRequest)
        {
            var existingNationality = await _repositoryManager.NationalityRepository
                .FindByCondition(n => n.Name == nationalityRequest.Name || n.NationalityCode == nationalityRequest.NationalityCode, false)
                .SingleOrDefaultAsync();

            if (existingNationality != null)
                throw new InvalidOperationException("Nationality with the specified name or code already exists");

            var nationality = _mapper.Map<Nationality>(nationalityRequest);
            await _repositoryManager.NationalityRepository.CreateAsync(nationality);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateNationalityAsync(short id, NationalityRequest nationalityRequest)
        {
            var nationality = await _repositoryManager.NationalityRepository.GetNationalityByIdAsync(id, true);
            if (nationality == null) throw new KeyNotFoundException("Nationality not found");

            var existingNationality = await _repositoryManager.NationalityRepository
                .FindByCondition(n => (n.Name == nationalityRequest.Name || n.NationalityCode == nationalityRequest.NationalityCode) && n.NationalityId != id, false)
                .SingleOrDefaultAsync();

            if (existingNationality != null)
                throw new InvalidOperationException("Nationality with the specified name or code already exists");

            _mapper.Map(nationalityRequest, nationality);
            _repositoryManager.NationalityRepository.Update(nationality);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteNationalityAsync(short id)
        {
            var nationality = await _repositoryManager.NationalityRepository.GetNationalityByIdAsync(id, false);
            if (nationality == null) throw new KeyNotFoundException("Nationality not found");

            var hasAuthors = await _repositoryManager.AuthorRepository
                .FindByCondition(a => a.Language!.NationalityId == id, false)
                .AnyAsync();

            if (hasAuthors)
                throw new InvalidOperationException("Cannot delete nationality with associated authors.");

            _repositoryManager.NationalityRepository.Delete(nationality);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<AuthorResponse>> GetAuthorsByNationalityIdAsync(short id, bool trackChanges)
        {
            var authors = await _repositoryManager.AuthorRepository
                .FindByCondition(a => a.Language!.NationalityId == id, trackChanges)
                .Include(a => a.AuthorBooks)!
                    .ThenInclude(ab => ab.Book)
                .ToListAsync();

            if (!authors.Any())
                throw new KeyNotFoundException("No authors found for this nationality");

            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }
    }
}
