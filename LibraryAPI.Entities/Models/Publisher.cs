using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PublisherId { get; set; }

        [Required(ErrorMessage = "Publisher name is required.")]
        [StringLength(800, ErrorMessage = "Publisher name cannot be longer than 800 characters.")]
        [Column(TypeName = "varchar(800)")]
        public string Name { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 15 characters.")]
        [Column(TypeName = "varchar(15)")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(320, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 320 characters.")]
        [Column(TypeName = "varchar(320)")]
        public string? Email { get; set; }

        [StringLength(800, ErrorMessage = "Contact person name cannot be longer than 800 characters.")]
        [Column(TypeName = "varchar(800)")]
        public string? ContactPerson { get; set; }

        public string PublisherStatus { get; set; } = Status.Active.ToString();

        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }

        [JsonIgnore]
        public ICollection<PublisherAddress>? Addresses { get; set; }
    }
}
