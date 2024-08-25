using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories.Concrete
{
    public class MemberAddressRepository : RepositoryBase<MemberAddress>, IMemberAddressRepository
    {
        public MemberAddressRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MemberAddress>> GetAllMemberAddressesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();

        public async Task<MemberAddress> GetMemberAddressByIdAsync(int id, bool trackChanges)
            => await FindByCondition(b => b.MemberAddressId.Equals(id), trackChanges).SingleOrDefaultAsync();

    }
}
