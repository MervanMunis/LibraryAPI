using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class BookLanguage
    {
        public short? LanguagesId { get; set; }

        [ForeignKey(nameof(LanguagesId))]
        public Language? Language { get; set; }


        public long? BooksId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BooksId))]
        public Book? Book { get; set; }
    }
}
