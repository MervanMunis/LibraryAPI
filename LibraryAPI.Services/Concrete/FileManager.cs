using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Services.Concrete
{
    public class FileManager : IFileService
    {
        private readonly string _baseDirectory;
        private readonly IRepositoryManager _repositoryManager;

        public FileManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "LibraryAPI");
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid");

            var folderPath = Path.Combine(_baseDirectory, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Flush();
            }

            return fileName;
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_baseDirectory, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<byte[]> GetImageByAuthorIdAsync(long authorId)
        {
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(authorId, false);
            if (author == null || string.IsNullOrEmpty(author.ImageFileName))
                throw new FileNotFoundException("Image not found for the specified author.");

            var filePath = Path.Combine(_baseDirectory, "AuthorImages", author.ImageFileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image not found for the specified author.");

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<byte[]> GetImageByBookIdAsync(long bookId)
        {
            var book = await _repositoryManager.BookRepository.GetBookByIdAsync(bookId, false);
            if (book == null || string.IsNullOrEmpty(book.CoverFileName))
                throw new FileNotFoundException("Image not found for the specified book.");

            var filePath = Path.Combine(_baseDirectory, "BookImages", book.CoverFileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image not found for the specified book.");

            return await File.ReadAllBytesAsync(filePath);
        }
    }
}
