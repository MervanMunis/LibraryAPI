using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class EmployeeAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeAddressId { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot be longer than 50 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters.")]
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [StringLength(20, ErrorMessage = "Postal code cannot be longer than 20 characters.")]
        [Column(TypeName = "varchar(20)")]
        public string PostalCode { get; set; } = string.Empty;


        public string EmployeeId { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}
