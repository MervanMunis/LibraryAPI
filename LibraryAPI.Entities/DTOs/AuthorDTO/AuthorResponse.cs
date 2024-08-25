namespace LibraryAPI.Entities.DTOs.AuthorDTO
{
    public class AuthorResponse
    {
        public long AuthorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Biography { get; set; }

        public short? BirthYear { get; set; }

        public string? DeathYear { get; set; }

        public string? AuthroStatus { get; set; }

        public List<string> Title { get; set; } = new List<string>();
    }
}
