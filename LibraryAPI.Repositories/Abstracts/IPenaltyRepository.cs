using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IPenaltyRepository : IRepositoryBase<Penalty>
    {
        Task<IEnumerable<Penalty>> GetPenaltiesByMemberIdAsync(string memberId, bool trackChanges);
        Task<Penalty> GetPenaltyByIdAsync(long penaltyId, bool trackChanges);
    }
}
