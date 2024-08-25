using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Entities.DTOs.PasswordDTO
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
