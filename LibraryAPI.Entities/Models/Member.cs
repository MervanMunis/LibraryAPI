using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class Member
    {
        [Key]
        public string MemberId { get; set; } = string.Empty;

        [ForeignKey(nameof(MemberId))]
        public ApplicationUser? ApplicationUser { get; set; }

        public string? MemberEducation { get; set; }

        public string? MemberStatus { get; set; }

        [JsonIgnore]
        public ICollection<Penalty>? Penalty { get; set; }

        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
}
