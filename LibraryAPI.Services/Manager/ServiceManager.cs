using AutoMapper;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using LibraryAPI.Services.Concrete;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Services.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthorService> _authorService;
        private readonly Lazy<IBookCopyService> _bookCopyService;
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IDepartmentService> _departmentService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IFileService> _fileService;
        private readonly Lazy<ILanguageService> _languageService;
        private readonly Lazy<ILoanService> _loanService;
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IMemberService> _memberService;
        private readonly Lazy<INationalityService> _nationalityService;
        private readonly Lazy<IPenaltyService> _penaltyService;
        private readonly Lazy<IPublisherService> _publisherService;
        private readonly Lazy<ISubCategoryService> _subCategoryService;
        private readonly Lazy<IWantedBookService> _wantedBookService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _fileService = new Lazy<IFileService>(() => new FileManager(repositoryManager));
            _penaltyService = new Lazy<IPenaltyService>(() => new PenaltyManager(repositoryManager, mapper));
            _authorService = new Lazy<IAuthorService>(() => new AuthorManager(repositoryManager, mapper, _fileService.Value));
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, mapper, _fileService.Value));
            _bookCopyService = new Lazy<IBookCopyService>(() => new BookCopyManager(repositoryManager, mapper));
            _categoryService = new Lazy<ICategoryService>(() => new CategoryManager(repositoryManager, mapper));
            _departmentService = new Lazy<IDepartmentService>(() => new DepartmentManager(repositoryManager, mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeManager(repositoryManager, mapper, userManager));
            _languageService = new Lazy<ILanguageService>(() => new LanguageManager(repositoryManager, mapper));
            _loanService = new Lazy<ILoanService>(() => new LoanManager(repositoryManager, mapper, _penaltyService.Value));
            _locationService = new Lazy<ILocationService>(() => new LocationManager(repositoryManager, mapper));
            _memberService = new Lazy<IMemberService>(() => new MemberManager(repositoryManager, mapper, userManager));
            _nationalityService = new Lazy<INationalityService>(() => new NationalityManager(repositoryManager, mapper));
            _publisherService = new Lazy<IPublisherService>(() => new PublisherManager(repositoryManager, mapper));
            _subCategoryService = new Lazy<ISubCategoryService>(() => new SubCategoryManager(repositoryManager, mapper));
            _wantedBookService = new Lazy<IWantedBookService>(() => new WantedBookManager(repositoryManager, mapper));
        }
        public IAuthorService AuthorService => _authorService.Value;
        public IBookCopyService BookCopyService => _bookCopyService.Value;
        public IBookService BookService => _bookService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public IDepartmentService DepartmentService => _departmentService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
        public ILanguageService LanguageService => _languageService.Value;
        public ILoanService LoanService => _loanService.Value;
        public ILocationService LocationService => _locationService.Value;
        public IMemberService MemberService => _memberService.Value;
        public INationalityService NationalityService => _nationalityService.Value;
        public IPenaltyService PenaltyService => _penaltyService.Value;
        public IPublisherService PublisherService => _publisherService.Value;
        public ISubCategoryService SubCategoryService => _subCategoryService.Value;
        public IWantedBookService WantedBookService => _wantedBookService.Value;
    }
}
