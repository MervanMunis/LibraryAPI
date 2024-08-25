using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.DTOs.LocationDTO
{
    public class LocationResponse
    {
        public int LocationId { get; set; }

        public string SectionCode { get; set; } = string.Empty;

        public string AisleCode { get; set; } = string.Empty;

        public string ShelfNumber { get; set; } = string.Empty;

        public string LocationStatus { get; set; } = Status.Active.ToString();
    }
}
