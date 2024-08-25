namespace LibraryAPI.Entities.DTOs.PublisherAddressDTO
{
    public class PublisherAddressRequest
    {
        public string? Street { get; set; }

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
