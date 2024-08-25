using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface INationalityRepository : IRepositoryBase<Nationality>
    {
        Task<IEnumerable<Nationality>> GetAllNationalitiesAsync(bool trackChanges);
        Task<Nationality> GetNationalityByIdAsync(short nationalityId, bool trackChanges);
    }
}
