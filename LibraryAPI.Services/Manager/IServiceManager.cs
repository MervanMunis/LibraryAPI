using LibraryAPI.Services.Abstracts;

namespace LibraryAPI.Services.Manager
{
    public interface IServiceManager
    {
        IAuthorService AuthorService { get; }
        IBookCopyService BookCopyService { get; }
        IBookService BookService { get; }
        ICategoryService CategoryService { get; }
        IDepartmentService DepartmentService { get; }
        IEmployeeService EmployeeService { get; }
        IFileService FileService { get; }
        ILanguageService LanguageService { get; }
        ILoanService LoanService { get; }
        ILocationService LocationService { get; }   
        IMemberService MemberService { get; }
        INationalityService NationalityService { get; }
        IPenaltyService PenaltyService { get; }
        IPublisherService PublisherService { get; }
        ISubCategoryService SubCategoryService { get; }
        IWantedBookService WantedBookService { get; }
    }
}
