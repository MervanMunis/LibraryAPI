namespace LibraryAPI.Entities.DTOs.MemberDTO
{
    public class MemberResponse
    {
        public string MemberId { get; set; } = string.Empty;

        public string IdNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public string? MemberEducation { get; set; }

        public string? MemberStatus { get; set; }

        public List<string>? PenaltyType { get; set; }

        public List<string>? LoanedBookNames { get; set; }
    }
}
