using LibraryAPI.Entities.DTOs.MemberDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Services.Abstracts
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberResponse>> GetAllMembersAsync(bool trackChanges);
        Task<MemberResponse> GetMemberByIdNumberAsync(string idNumber, bool trackChanges);
        Task<MemberResponse> GetMemberByIdAsync(string id, bool trackChanges);
        Task AddMemberAsync(MemberRequest memberRequest);
        Task UpdateMemberAsync(string id, MemberRequest memberRequest);
        Task SetMemberStatusAsync(string idNumber, string status);
        Task AddMemberAddressAsync(MemberAddress memberAddress);
        Task UpdateMemberPasswordAsync(string id, string currentPassword, string newPassword);
    }
}
