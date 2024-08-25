using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short LanguageId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Column(TypeName = "char(100)")]
        public string Name { get; set; } = string.Empty;

        public string LanguageStatus { get; set; } = Status.Active.ToString();


        public short NationalityId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(NationalityId))]
        public Nationality? Nationality { get; set; }


        [JsonIgnore]
        public ICollection<BookLanguage>? BookLanguages { get; set; }
    }
}
