using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryAPI.Entities.Enums;

namespace LibraryAPI.Entities.Models
{
    public class Penalty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PenaltyId { get; set; }

        [Required(ErrorMessage = "Daily fee is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, float.MaxValue, ErrorMessage = "Daily fee must be a positive value.")]
        public decimal DailyFee { get; set; } = 0.5M;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, float.MaxValue, ErrorMessage = "Total fee must be a positive value.")]
        public decimal? TotalFee { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Overdue days must be a positive value.")]
        public int? OverdueDays { get; set; }

        [Required(ErrorMessage = "Penalty type is required.")]
        [Column(TypeName = "varchar(50)")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Member ID is required.")]
        public string MemberId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }

        // Method to calculate penalty details based on the overdue days and end date
        public void CalculatePenaltyDetails()
        {
            if (EndDate.HasValue)
            {
                OverdueDays = (EndDate.Value - StartDate).Days;
                TotalFee = DailyFee * OverdueDays;

                if (OverdueDays > 30 && OverdueDays <= 60)
                {
                    Type = PenaltyType.BookTwoMonths.ToString();
                }
                else if (OverdueDays > 60 && OverdueDays <= 365)
                {
                    Type = PenaltyType.BookOneYear.ToString();
                }
                else if (OverdueDays > 365)
                {
                    Type = PenaltyType.BookLimitless.ToString();
                }
                else if (OverdueDays > 10)
                {
                    Type = PenaltyType.BookTenDays.ToString();
                }
                else
                {
                    Type = PenaltyType.None.ToString();
                }
            }
        }
    }
}
