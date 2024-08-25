using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.DTOs.LoanDTO
{
    public class LoanRequest
    {
        public short HowManyDays { get; set; }

        public long BookCopyId { get; set; }

        public long BookId { get; set; }

        public string? MemberIdNumber { get; set; }

        [JsonIgnore]
        public string MemberId { get; set; } = string.Empty;

        [JsonIgnore]
        public string EmployeeId { get; set; } = string.Empty;
    }
}
