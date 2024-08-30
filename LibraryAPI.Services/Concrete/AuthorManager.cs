using AutoMapper;
using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using LibraryAPI.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class AuthorManager : IAuthorService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AuthorManager(IRepositoryManager repositoryManager, IMapper mapper, IFileService fileService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync(bool trackChanges)
        {
            var authors = await _repositoryManager.AuthorRepository
                .GetAllAuthorsAsync(trackChanges);
            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllActiveAuthorsAsync(bool trackChanges)
        {
            var authors = await _repositoryManager.AuthorRepository
                .GetAllActiveAuthorsAsync(Status.Active.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllInActiveAuthorsAsync(bool trackChanges)
        {
            var authors = await _repositoryManager.AuthorRepository
                .GetAllInActiveAuthorsAsync(Status.InActive.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllBannedAuthorsAsync(bool trackChanges)
        {
            var authors = await _repositoryManager.AuthorRepository.GetAllBannedAuthorsAsync(Status.Banned.ToString(), trackChanges);
            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }

        public async Task<AuthorResponse> GetAuthorByIdAsync(long id, bool trackChanges)
        {
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(id, trackChanges);
            if (author == null)
                throw new NotFoundException("Author not found");

            if (author.AuthroStatus != Status.Active.ToString())
                throw new ConflictException("The author is not active anymore!");

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<string> AddAuthorAsync(AuthorRequest authorRequest)
        {
            // Validate the author's death year
            if (authorRequest.DeathYear.HasValue && authorRequest.DeathYear > DateTime.Now.Year)
                throw new BadRequestException("The author's death year cannot be in the future!");

            if (authorRequest.BirthYear.HasValue && authorRequest.BirthYear < DateTime.Now.Year - 10)
                throw new BadRequestException("The author's age cannot be smaller than 10 years!");

            if (await _repositoryManager.LanguageRepository.GetLanguageByIdAsync(authorRequest.LanguageId, false) == null)
                throw new BadRequestException($"The specified LanguageId: {authorRequest.LanguageId} does not exist!");


            // Check if an author with the same details already exists
            var authorExists = await _repositoryManager.AuthorRepository
                .FindByCondition(a => a.FullName == authorRequest.FullName &&
                                      a.BirthYear == authorRequest.BirthYear &&
                                      a.DeathYear == authorRequest.DeathYear, false)
                .AnyAsync();

            if (authorExists)
                throw new ConflictException("The Author already exists!");

            var author = _mapper.Map<Author>(authorRequest);

            await _repositoryManager.AuthorRepository.CreateAsync(author);
            await _repositoryManager.SaveAsync();

            return "Author successfully created!";
        }

        public async Task<bool> UpdateAuthorAsync(long id, AuthorRequest authorRequest, bool trackChanges)
        {
            // Retrieve the author by ID and track changes if necessary
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(id, trackChanges);
            if (author == null)
                throw new NotFoundException("Author not found");

            // Validate the author's death year
            if (authorRequest.DeathYear.HasValue && authorRequest.DeathYear > DateTime.Now.Year)
                throw new BadRequestException("The author's death year cannot be in the future!");

            // Check if another author with the same details already exists
            var authorExists = await _repositoryManager.AuthorRepository
                .FindByCondition(a => a.FullName == authorRequest.FullName &&
                                      a.BirthYear == authorRequest.BirthYear &&
                                      a.DeathYear == authorRequest.DeathYear &&
                                      a.AuthorId != id, false)
                .AnyAsync();

            if (authorExists)
                throw new ConflictException("An author with the same details already exists!");

            // Map the updated values to the existing author entity
            _mapper.Map(authorRequest, author);

            // Update the author in the repository and save the changes
            _repositoryManager.AuthorRepository.Update(author);
            await _repositoryManager.SaveAsync();

            return true;
        }
        public async Task<bool> SetAuthorStatusAsync(long id, string status)
        {
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(id, true);
            if (author == null)
                throw new NotFoundException("Author not found");

            author.AuthroStatus = status;
            _repositoryManager.AuthorRepository.Update(author);

            // Perform a bulk update manually
            var booksToUpdate = await _repositoryManager.BookRepository
                .FindByCondition(b => b.AuthorBooks!.Any(ab => ab.AuthorsId == id), true)
                .ToListAsync();

            foreach (var book in booksToUpdate)
            {
                book.BookStatus = status;
            }

            _repositoryManager.BookRepository.UpdateRange(booksToUpdate);

            await _repositoryManager.SaveAsync();

            return true;
        }


        public async Task<bool> UpdateAuthorImageAsync(long id, IFormFile image)
        {
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(id, true);
            if (author == null)
                throw new NotFoundException("Author not found");

            var fileName = await _fileService.SaveFileAsync(image, "AuthorImages");

            author.ImageFileName = fileName;
            _repositoryManager.AuthorRepository.Update(author);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<byte[]> GetAuthorImageAsync(long authorId)
        {
            var author = await _repositoryManager.AuthorRepository.GetAuthorByIdAsync(authorId, true);
            if (author == null)
                throw new NotFoundException("Author not found");

            try
            {
                return await _fileService.GetImageByAuthorIdAsync(authorId);
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
