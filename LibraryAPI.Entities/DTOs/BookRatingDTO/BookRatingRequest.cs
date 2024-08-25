using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.DTOs.BookRatingDTO
{
    public class BookRatingRequest
    {
        public float GivenRating { get; set; }

        [JsonIgnore]
        public string? MemberId { get; set; }
    }
}
