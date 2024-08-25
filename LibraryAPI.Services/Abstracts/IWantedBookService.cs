using LibraryAPI.Entities.DTOs.WantedBookDTO;

namespace LibraryAPI.Services.Abstracts
{
    public interface IWantedBookService
    {
        Task<IEnumerable<WantedBookResponse>> GetAllWantedBooksAsync(bool trackChanges);
        Task<WantedBookResponse> GetWantedBookByIdAsync(int id, bool trackChanges);
        Task<string> AddWantedBookAsync(WantedBookRequest wantedBookRequest);
        Task<bool> UpdateWantedBookAsync(int id, WantedBookRequest wantedBookRequest);
        Task<bool> DeleteWantedBookAsync(int id);
    }
}
