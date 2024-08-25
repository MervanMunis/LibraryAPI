    namespace LibraryAPI.Entities.DTOs.NationalityDTO
{
    public class NationalityResponse
    {
        public short NationalityId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string NationalityCode { get; set; } = string.Empty;
    }
}
