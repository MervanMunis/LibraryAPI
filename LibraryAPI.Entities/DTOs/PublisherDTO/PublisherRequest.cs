namespace LibraryAPI.Entities.DTOs.PublisherDTO
{
    public class PublisherRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? ContactPerson { get; set; }
    }
}
