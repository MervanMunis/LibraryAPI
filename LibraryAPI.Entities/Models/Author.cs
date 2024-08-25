using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuthorId { get; set; }

        [Required]
        [StringLength(800)]
        [Column(TypeName = "nvarchar(800)")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "The biography cannot be longer than 1000 character")]
        [Column(TypeName = "nvarchar(1000)")]
        public string? Biography { get; set; }

        [Range(-4000, 2100)]
        public short? BirthYear { get; set; }

        [Range(-4000, 2100)]
        public short? DeathYear { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string AuthroStatus { get; set; } = Status.Active.ToString();

        [Column(TypeName = "nvarchar(200)")]
        public string? ImageFileName { get; set; }

        public short LanguageId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; }

        [JsonIgnore]
        public ICollection<AuthorBook>? AuthorBooks { get; set; }

        public Author(string fullName, string? biography, short? birthYear, short? deathYear, short languageId)
        {
            FullName = fullName;
            Biography = biography;
            BirthYear = birthYear;
            DeathYear = deathYear;
            LanguageId = languageId;
        }
    }
}
