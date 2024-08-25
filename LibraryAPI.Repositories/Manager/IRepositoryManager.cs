using LibraryAPI.Repositories.Abstracts;

namespace LibraryAPI.Repositories.Manager
{
    public interface IRepositoryManager
    {
        IAuthorBookRepository AuthorBookRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IBookCopyRepository BookCopyRepository { get; }
        IBookLanguageRepository BookLanguageRepository { get; }
        IBookRatingRepository BookRatingRepository { get; }
        IBookRepository BookRepository { get; }
        IBookSubCategoryRepository BookSubCategoryRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDepartmentRepository DepartmentRepository { get; } 
        IEmployeeAddressRepository EmployeeAddressRepository { get; }
        IEmployeeRepository EmployeeRepository { get; } 
        ILanguageRepository LanguageRepository { get; }
        ILoanRepository LoanRepository { get; }
        ILoanTransactionRepository LoanTransactionRepository { get; }
        ILocationRepository LocationRepository { get; }
        IMemberAddressRepository MemberAddressRepository { get; }   
        IMemberRepository MemberRepository { get; }
        INationalityRepository NationalityRepository { get; }
        IPenaltyRepository PenaltyRepository { get; }
        IPublisherAddressRepository PublisherAddressRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        ISubCategoryRepository SubCategoryRepository { get; }
        IWantedBookRepository WantedBookRepository { get; }

        Task SaveAsync();
    }
}
