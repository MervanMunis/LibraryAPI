using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public ApplicationUser? ApplicationUser { get; set; }

        public float Salary { get; set; }

        public string EmployeeShift { get; set; } = Shift.Morning.ToString();

        [Required]
        public string? EmployeeTitle { get; set; }

        public string Status { get; set; } = EmployeeStatus.Working.ToString();

        public short DepartmentId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [JsonIgnore]
        public ICollection<EmployeeAddress>? EmployeeAddresses { get; set; }

        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }

        [JsonIgnore]
        public ICollection<LoanTransaction>? LoanTransactions { get; set; }
    }
}
