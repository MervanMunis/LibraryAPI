using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BookId { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN cannot be longer than 13 characters.")]
        [Column(TypeName = "varchar(13)")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Range(1, short.MaxValue, ErrorMessage = "Page number must be greater than zero.")]
        public int PageCount { get; set; }

        [Required]
        [Range(-4000, 2100)]
        public short PublishingYear { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        [Column(TypeName = "nvarchar(2000)")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Print count must be positive.")]
        public int? PrintCount { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string BookStatus { get; set; } = Status.Active.ToString();

        [Column(TypeName = "nvarchar(200)")]
        public string? CoverFileName { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0-5")]
        public float? Rating { get; set; } = 0;

        // Navigation Properties

        public long? PublisherId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PublisherId))]
        public Publisher? Publisher { get; set; }

        [JsonIgnore]
        public ICollection<BookSubCategory>? BookSubCategories { get; set; }

        [JsonIgnore]
        public ICollection<AuthorBook>? AuthorBooks { get; set; }

        [JsonIgnore]
        public ICollection<BookLanguage>? BookLanguages { get; set; }

        [JsonIgnore]
        public ICollection<BookCopy>? BookCopies { get; set; }

        [JsonIgnore]
        public ICollection<BookRating>? BookRatings { get; set; }
    }
}
