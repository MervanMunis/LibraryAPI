using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class WantedBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WantedBookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        [Column(TypeName = "nvarchar(2000)")]
        public string? Description { get; set; }


        public short LanguageId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; }

        public short CategoryId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
