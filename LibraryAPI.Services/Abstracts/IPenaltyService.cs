using LibraryAPI.Entities.DTOs.LoanDTO;
using LibraryAPI.Entities.DTOs.PenaltyDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface IPenaltyService
    {
        Task<IEnumerable<PenaltyResponse>> GetPenaltiesByMemberIdAsync(string memberId, bool trackChanges);
        Task<PenaltyResponse> GetPenaltyByIdAsync(long penaltyId, bool trackChanges);
        Task CalculatePenaltiesAsync(LoanReturnRequest loanReturnRequest);
    }
}
