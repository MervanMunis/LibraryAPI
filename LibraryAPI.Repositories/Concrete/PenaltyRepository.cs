using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class PenaltyRepository : RepositoryBase<Penalty>, IPenaltyRepository
    {
        public PenaltyRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Penalty>> GetPenaltiesByMemberIdAsync(string memberId, bool trackChanges)
        {
            return await FindByCondition(p => p.MemberId.Equals(memberId), trackChanges)
                .Include(p => p.Member)
                .ThenInclude(m => m!.ApplicationUser)
                .ToListAsync();
        }

        public async Task<Penalty> GetPenaltyByIdAsync(long penaltyId, bool trackChanges)
        {
            return await FindByCondition(p => p.PenaltyId == penaltyId, trackChanges)
                .Include(p => p.Member)
                .ThenInclude(m => m.ApplicationUser)
                .SingleOrDefaultAsync();
        }
    }
}
