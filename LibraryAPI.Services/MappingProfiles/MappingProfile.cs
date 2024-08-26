using AutoMapper;
using LibraryAPI.Entities.DTOs.AuthorDTO;
using LibraryAPI.Entities.DTOs.BookDTO;
using LibraryAPI.Entities.DTOs.CategoryDTO;
using LibraryAPI.Entities.DTOs.DepartmentDTO;
using LibraryAPI.Entities.DTOs.EmployeeDTO;
using LibraryAPI.Entities.DTOs.LanguageDTO;
using LibraryAPI.Entities.DTOs.LoanDTO;
using LibraryAPI.Entities.DTOs.LocationDTO;
using LibraryAPI.Entities.DTOs.MemberDTO;
using LibraryAPI.Entities.DTOs.NationalityDTO;
using LibraryAPI.Entities.DTOs.PenaltyDTO;
using LibraryAPI.Entities.DTOs.PublisherAddressDTO;
using LibraryAPI.Entities.DTOs.PublisherDTO;
using LibraryAPI.Entities.DTOs.SubCategoryDTO;
using LibraryAPI.Entities.DTOs.WantedBookDTO;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Author mappings
            CreateMap<Author, AuthorResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.AuthorBooks != null
                    ? src.AuthorBooks.Select(ab => ab.Book!.Title).ToList()
                    : new List<string>()));
            CreateMap<AuthorRequest, Author>()
                .ForMember(dest => dest.AuthorBooks, opt => opt.Ignore());

            // Penalty mappings
            CreateMap<Penalty, PenaltyResponse>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member!.ApplicationUser!.Name));
            CreateMap<PenaltyRequest, Penalty>();

            // Book mappings
            CreateMap<Book, BookResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.BookSubCategories != null && src.BookSubCategories.Any()
                    ? src.BookSubCategories.FirstOrDefault()!.SubCategory!.Category!.Name
                    : string.Empty))
                .ForMember(dest => dest.SubCategoryNames, opt => opt.MapFrom(src => src.BookSubCategories != null
                    ? src.BookSubCategories.Select(sc => sc.SubCategory!.Name).ToList()
                    : new List<string>()))
                .ForMember(dest => dest.LanguageNames, opt => opt.MapFrom(src => src.BookLanguages != null
                    ? src.BookLanguages.Select(l => l.Language!.Name).ToList()
                    : new List<string>()))
                .ForMember(dest => dest.AuthorNames, opt => opt.MapFrom(src => src.AuthorBooks != null
                    ? src.AuthorBooks.Select(ab => ab.Author!.FullName).ToList()
                    : new List<string>()))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null
                    ? src.Publisher.Name
                    : string.Empty));
            CreateMap<BookRequest, Book>();

            // Category mappings
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<CategoryRequest, Category>();

            // Department mappings
            CreateMap<Department, DepartmentResponse>().ReverseMap();
            CreateMap<DepartmentRequest, Department>();

            // Employee mappings
            CreateMap<Employee, EmployeeResponse>().ReverseMap();
            CreateMap<EmployeeRequest, Employee>();

            // Language mappings
            CreateMap<Language, LanguageResponse>().ReverseMap();
            CreateMap<LanguageRequest, Language>();

            // Loan mappings
            CreateMap<Loan, LoanResponse>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookCopy!.Book!.Title))
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member!.ApplicationUser!.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.ApplicationUser!.Name));
            
            CreateMap<LoanRequest, Loan>()
                .ForMember(dest => dest.MemberId, opt => opt.Ignore()) // Ignored because it's set in the service
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()); // Ignored because it's set in the service
            
            CreateMap<LoanUpdateRequest, Loan>();
            CreateMap<LoanTransaction, LoanTransactionResponse>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee!.ApplicationUser!.Name));

            // Location mappings
            CreateMap<Location, LocationResponse>().ReverseMap();
            CreateMap<LocationRequest, Location>();

            // Member mappings
            CreateMap<Member, MemberResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ApplicationUser!.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser!.LastName))
                .ForMember(dest => dest.MemberEducation, opt => opt.MapFrom(src => src.MemberEducation))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.ApplicationUser!.RegisterDate));
            CreateMap<MemberRequest, Member>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore()); // ApplicationUser is set in the service

            // Nationality mappings
            CreateMap<Nationality, NationalityResponse>().ReverseMap();
            CreateMap<NationalityRequest, Nationality>();

            // Publisher mappings
            CreateMap<Publisher, PublisherResponse>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books!.Select(b => b.Title).ToList()))
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses!.Select(a => new PublisherAddressResponse
                {
                    PublisherAddressId = a.PublisherAddressId,
                    Street = a.Street,
                    City = a.City,
                    Country = a.Country,
                    PostalCode = a.PostalCode
                }).ToList()));
            CreateMap<PublisherRequest, Publisher>();
            CreateMap<PublisherAddress, PublisherAddressResponse>().ReverseMap();
            CreateMap<PublisherAddressRequest, PublisherAddress>();

            // SubCategory mappings
            CreateMap<SubCategory, SubCategoryResponse>().ReverseMap();
            CreateMap<SubCategoryRequest, SubCategory>();

            // WantedBook mappings
            CreateMap<WantedBook, WantedBookResponse>()
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Language!.Name))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.Category!.SubCategories!.Select(sc => sc.Name).ToList()));
            CreateMap<WantedBookRequest, WantedBook>();


        }
    }
}
