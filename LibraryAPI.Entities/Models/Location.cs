using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Section code is required.")]
        [StringLength(1000, ErrorMessage = "Section code cannot be longer than 1000 characters.")]
        [Column(TypeName = "varchar(1000)")]
        public string SectionCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Aisle code is required.")]
        [StringLength(100, ErrorMessage = "Aisle code cannot be longer than 100 characters.")]
        [Column(TypeName = "varchar(100)")]
        public string AisleCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shelf number is required.")]
        [StringLength(6, MinimumLength = 3, ErrorMessage = "Shelf number must be between 3 and 6 characters.")]
        [Column(TypeName = "varchar(6)")]
        public string ShelfNumber { get; set; } = string.Empty;

        public string LocationStatus { get; set; } = Status.Active.ToString();

        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }
    }
}
