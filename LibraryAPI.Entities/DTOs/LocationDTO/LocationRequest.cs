namespace LibraryAPI.Entities.DTOs.LocationDTO
{
    public class LocationRequest
    {
        public string SectionCode { get; set; } = string.Empty;

        public string AisleCode { get; set; } = string.Empty;

        public string ShelfNumber { get; set; } = string.Empty;
    }
}
