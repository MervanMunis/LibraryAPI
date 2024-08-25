using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Entities.DTOs.PasswordDTO
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
