using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Entities.DTOs.PasswordDTO
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
