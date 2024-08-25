using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Entities.Models;

namespace LibraryAPI.Repositories.Data
{
    public class RepositoryContext : IdentityDbContext<ApplicationUser>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Location>? Locations { get; set; }
        public DbSet<SubCategory>? SubCategories { get; set; }
        public DbSet<Language>? Languages { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Publisher>? Publishers { get; set; }
        public DbSet<PublisherAddress>? PublisherAddresses { get; set; }
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Nationality>? Nationalities { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<BookRating>? BookRatings { get; set; }
        public DbSet<BookCopy>? BookCopies { get; set; }
        public DbSet<AuthorBook>? AuthorBooks { get; set; }
        public DbSet<BookLanguage>? BookLanguages { get; set; }
        public DbSet<BookSubCategory>? BookSubCategories { get; set; }
        public DbSet<Loan>? Loans { get; set; }
        public DbSet<Penalty>? Penalties { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<WantedBook>? WantedBooks { get; set; }
        public DbSet<Member>? Members { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<EmployeeAddress>? EmployeeAddresses { get; set; }
        public DbSet<MemberAddress>? MemberAddresses { get; set; }
        public DbSet<LoanTransaction>? LoanTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and composite keys

            modelBuilder.Entity<AuthorBook>()
                .HasKey(ab => new { ab.AuthorsId, ab.BooksId });

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Author)
                .WithMany(a => a.AuthorBooks)
                .HasForeignKey(ab => ab.AuthorsId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Book)
                .WithMany(b => b.AuthorBooks)
                .HasForeignKey(ab => ab.BooksId);

            modelBuilder.Entity<BookLanguage>()
                .HasKey(bl => new { bl.LanguagesId, bl.BooksId });

            modelBuilder.Entity<BookLanguage>()
                .HasOne(bl => bl.Language)
                .WithMany(l => l.BookLanguages)
                .HasForeignKey(bl => bl.LanguagesId);

            modelBuilder.Entity<BookLanguage>()
                .HasOne(bl => bl.Book)
                .WithMany(b => b.BookLanguages)
                .HasForeignKey(bl => bl.BooksId);

            modelBuilder.Entity<BookSubCategory>()
                .HasKey(bsc => new { bsc.BooksId, bsc.SubCategoriesId });

            modelBuilder.Entity<BookSubCategory>()
                .HasOne(bsc => bsc.Book)
                .WithMany(b => b.BookSubCategories)
                .HasForeignKey(bsc => bsc.BooksId);

            modelBuilder.Entity<BookSubCategory>()
                .HasOne(bsc => bsc.SubCategory)
                .WithMany(sc => sc.BookSubCategories)
                .HasForeignKey(bsc => bsc.SubCategoriesId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.Loans)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.BookCopy)
                .WithMany(bc => bc.Loans)
                .HasForeignKey(l => l.BookCopyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanTransaction>()
                .HasKey(lt => new { lt.LoanId, lt.EmployeeId });

            modelBuilder.Entity<LoanTransaction>()
                .HasOne(lt => lt.Loan)
                .WithMany(l => l.LoanTransactions)
                .HasForeignKey(lt => lt.LoanId);

            modelBuilder.Entity<LoanTransaction>()
                .HasOne(lt => lt.Employee)
                .WithMany(e => e.LoanTransactions)
                .HasForeignKey(lt => lt.EmployeeId);
        }
    }
}
