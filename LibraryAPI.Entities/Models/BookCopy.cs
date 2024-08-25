using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class BookCopy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BookCopyId { get; set; }

        public string BookCopyStatus { get; set; } = Status.Active.ToString();

        public long BookId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

        public int? LocationId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(LocationId))]
        public Location? Location { get; set; }

        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
}
