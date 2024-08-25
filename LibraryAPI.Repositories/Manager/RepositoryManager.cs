using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Concrete;
using LibraryAPI.Repositories.Data;

namespace LibraryAPI.Repositories.Manager
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private IAuthorBookRepository _authorBookRepository;
        private IAuthorRepository _authorRepository;
        private IBookCopyRepository _bookCopyRepository;
        private IBookLanguageRepository _bookLanguageRepository;
        private IBookRepository _bookRepository;
        private IBookSubCategoryRepository _bookSubCategoryRepository;
        private IBookRatingRepository _bookRatingRepository;
        private ICategoryRepository _categoryRepository;
        private IDepartmentRepository _departmentRepository;
        private IEmployeeAddressRepository _employeeAddressRepository;
        private IEmployeeRepository _employeeRepository;
        private ILanguageRepository _languageRepository;
        private ILoanRepository _loanRepository;
        private ILoanTransactionRepository _loanTransactionRepository;
        private ILocationRepository _locationRepository;
        private IMemberRepository _memberRepository;
        private IMemberAddressRepository _memberAddressRepository;
        private INationalityRepository _nationalityRepository;
        private IPenaltyRepository _penaltyRepository;
        private IPublisherAddressRepository _publisherAddressRepository;
        private IPublisherRepository _publisherRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private IWantedBookRepository _wantedBookRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        public IAuthorRepository AuthorRepository =>
            _authorRepository ??= new AuthorRepository(_context);

        public IBookCopyRepository BookCopyRepository => 
            _bookCopyRepository ??= new BookCopyRepository(_context);


        public IBookRepository BookRepository =>
            _bookRepository ??= new BookRepository(_context);

        public IAuthorBookRepository AuthorBookRepository =>
            _authorBookRepository ??= new AuthorBookRepository(_context);

        public IBookLanguageRepository BookLanguageRepository =>
            _bookLanguageRepository ??= new BookLanguageRepository(_context);

        public IBookSubCategoryRepository BookSubCategoryRepository =>
            _bookSubCategoryRepository ??= new BookSubCategoryRepository(_context);

        public IBookRatingRepository BookRatingRepository =>
            _bookRatingRepository ??= new BookRatingRepository(_context);

        public ICategoryRepository CategoryRepository =>
            _categoryRepository ??= new CategoryRepository(_context);

        public IDepartmentRepository DepartmentRepository => 
            _departmentRepository ??= new DepartmentRepository(_context);

        public IEmployeeAddressRepository EmployeeAddressRepository => 
            _employeeAddressRepository ??= new EmployeeAddressRepository(_context);

        public IEmployeeRepository EmployeeRepository => 
            _employeeRepository ??= new EmployeeRepository(_context);

        public ILanguageRepository LanguageRepository => 
            _languageRepository ??= new LanguageRepository(_context);

        public ILoanRepository LoanRepository => 
            _loanRepository ??= new LoanRepository(_context);

        public ILoanTransactionRepository LoanTransactionRepository => 
            _loanTransactionRepository ??= new LoanTransactionRepository(_context);

        public ILocationRepository LocationRepository => 
            _locationRepository ??= new LocationRepository(_context);

        public IMemberAddressRepository MemberAddressRepository => 
            _memberAddressRepository ??= new MemberAddressRepository(_context);

        public IMemberRepository MemberRepository => 
            _memberRepository ??= new MemberRepository(_context);

        public INationalityRepository NationalityRepository =>
            _nationalityRepository ??= new NationalityRepository(_context);

        public IPenaltyRepository PenaltyRepository => 
            _penaltyRepository ??= new PenaltyRepository(_context);

        public IPublisherAddressRepository PublisherAddressRepository => 
            _publisherAddressRepository ??= new PublisherAddressRepository(_context);

        public IPublisherRepository PublisherRepository =>
            _publisherRepository ??= new PublisherRepository(_context);

        public ISubCategoryRepository SubCategoryRepository => 
            _subCategoryRepository ??= new SubCategoryRepository(_context);

        public IWantedBookRepository WantedBookRepository =>
            _wantedBookRepository ??= new WantedBookRepository(_context);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
