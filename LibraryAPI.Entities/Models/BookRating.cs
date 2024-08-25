using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class BookRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BookRatingId { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0-5")]
        public float? GivenRating { get; set; } = 0;

        public string MemberId { get; set; } = string.Empty;

        public long BookId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }
    }
}
