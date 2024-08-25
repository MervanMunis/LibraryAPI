using LibraryAPI.Entities.DTOs.AuthorDTO;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Services.Abstracts
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync(bool trackChanges);
        Task<IEnumerable<AuthorResponse>> GetAllActiveAuthorsAsync(bool trackChanges);
        Task<IEnumerable<AuthorResponse>> GetAllInActiveAuthorsAsync(bool trackChanges);
        Task<IEnumerable<AuthorResponse>> GetAllBannedAuthorsAsync(bool trackChanges);
        Task<AuthorResponse> GetAuthorByIdAsync(long id, bool trackChanges);
        Task<string> AddAuthorAsync(AuthorRequest authorRequest);
        Task<bool> UpdateAuthorAsync(long id, AuthorRequest authorRequest);
        Task<bool> SetAuthorStatusAsync(long id, string status);
        Task<bool> UpdateAuthorImageAsync(long id, IFormFile image);
        Task<byte[]> GetAuthorImageAsync(long authorId);
    }
}
