using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Entities.DTOs.NationalityDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityResponse>> GetAllNationalitiesAsync(bool trackChanges);
        Task<NationalityResponse> GetNationalityByIdAsync(short id, bool trackChanges);
        Task AddNationalityAsync(NationalityRequest nationalityRequest);
        Task UpdateNationalityAsync(short id, NationalityRequest nationalityRequest);
        Task DeleteNationalityAsync(short id);
        Task<IEnumerable<AuthorResponse>> GetAuthorsByNationalityIdAsync(short id, bool trackChanges);
    }
}
