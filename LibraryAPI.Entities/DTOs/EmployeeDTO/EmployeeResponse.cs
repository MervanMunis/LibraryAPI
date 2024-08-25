namespace LibraryAPI.Entities.DTOs.EmployeeDTO
{
    public class EmployeeResponse
    {
        public string EmployeeId { get; set; } = string.Empty;

        public string IdNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }

        public float Salary { get; set; }

        public string? EmployeeShift { get; set; }

        public string? EmployeeTitle { get; set; }
    }
}
