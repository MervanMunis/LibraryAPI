using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Entities.DTOs.MemberDTO
{
    public class MemberRequest
    {
        public string IdNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string? EducationalDegree { get; set; }

        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
