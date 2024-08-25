using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }

        [Required(ErrorMessage = "Loaned date is required.")]
        [DataType(DataType.Date)]
        public DateTime LoanedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Count of days is required.")]
        [Range(1, 365, ErrorMessage = "Count of days must be between 1 and 365.")]
        public short CountDay { get; set; } = 20;

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Book status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot be longer than 20 characters.")]
        [Column(TypeName = "varchar(20)")]
        public string LoanStatus { get; set; } = Status.Borrowed.ToString();

        [Required(ErrorMessage = "Member ID is required.")]
        public string MemberId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        [Required(ErrorMessage = "BookCopy ID is required.")]
        [ForeignKey(nameof(BookCopy))]
        public long BookCopyId { get; set; }

        [JsonIgnore]
        public BookCopy? BookCopy { get; set; }

        [JsonIgnore]
        public ICollection<LoanTransaction>? LoanTransactions { get; set; }

        public Loan()
        {
            // Default constructor for EF
        }

        public Loan(DateTime loanedDate, short countDay, string memberId, string employeeId, long bookCopyId)
        {
            LoanedDate = loanedDate;
            CountDay = countDay;
            MemberId = memberId;
            EmployeeId = employeeId;
            BookCopyId = bookCopyId;
            LoanStatus = Status.Borrowed.ToString();
        }
    }
}
