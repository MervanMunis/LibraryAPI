using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string IdNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}
