using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class Nationality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short NationalityId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NationalityCode is required.")]
        [Column(TypeName = "varchar(3)")]
        [StringLength(3, ErrorMessage = "NationalityCode cannot be longer than 4 chracter.")]
        public string NationalityCode { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Language>? Languages { get; set; }

        [JsonIgnore]
        public ICollection<AuthorBook>? Authors { get; set; }
    }
}
