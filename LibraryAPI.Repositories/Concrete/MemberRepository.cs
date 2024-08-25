using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class MemberRepository : RepositoryBase<Member>, IMemberRepository
    {
        public MemberRepository(RepositoryContext context) : base(context) { }

        public async Task<IEnumerable<Member>> GetAllMembersAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<Member> GetMemberByIdAsync(string memberId, bool trackChanges) =>
            await FindByCondition(m => m.MemberId.Equals(memberId), trackChanges).SingleOrDefaultAsync();

        public async Task<Member> GetMemberByIdNumberAsync(string idNumber, bool trackChanges) =>
            await FindByCondition(m => m.MemberId.Equals(idNumber), trackChanges).SingleOrDefaultAsync();
        
    }
}
