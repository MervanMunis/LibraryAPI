using LibraryAPI.Entities.DTOs.PasswordDTO;
using LibraryAPI.Presentation.Auth.Models;

namespace LibraryAPI.Presentation.Auth.Services
{
    public interface IAuthenticationService
    {
        Task<string> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<string> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<string> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
    }
}
