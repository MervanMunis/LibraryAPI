using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IMemberRepository : IRepositoryBase<Member>
    {
        Task<IEnumerable<Member>> GetAllMembersAsync(bool trackChanges);
        Task<Member> GetMemberByIdAsync(string memberId, bool trackChanges);
        Task<Member> GetMemberByIdNumberAsync(string idNumber, bool trackChanges);
    }
}
