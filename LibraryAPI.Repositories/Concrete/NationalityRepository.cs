using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class NationalityRepository : RepositoryBase<Nationality>, INationalityRepository
    {
        public NationalityRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Nationality>> GetAllNationalitiesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Nationality> GetNationalityByIdAsync(short nationalityId, bool trackChanges) =>
            await FindByCondition(n => n.NationalityId.Equals(nationalityId), trackChanges).SingleOrDefaultAsync();
    }
}
