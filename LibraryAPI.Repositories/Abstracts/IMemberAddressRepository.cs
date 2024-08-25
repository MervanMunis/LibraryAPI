using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IMemberAddressRepository : IRepositoryBase<MemberAddress>
    {
        Task<IEnumerable<MemberAddress>> GetAllMemberAddressesAsync(bool trackChanges);
        Task<MemberAddress> GetMemberAddressByIdAsync(int id, bool trackChanges);
    }
}
