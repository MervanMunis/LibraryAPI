using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class AuthorBook
    {
        public long? AuthorsId { get; set; }

        [ForeignKey(nameof(AuthorsId))]
        public Author? Author { get; set; }

        public long? BooksId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BooksId))]
        public Book? Book { get; set; }
    }
}
