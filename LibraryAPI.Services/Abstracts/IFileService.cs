using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Services.Abstracts
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> GetImageByAuthorIdAsync(long authorId);
        Task<byte[]> GetImageByBookIdAsync(long bookId);
    }
}
