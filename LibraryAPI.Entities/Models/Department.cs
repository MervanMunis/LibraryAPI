using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.Entities.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot be longer than 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Employee>? Employees { get; set; }
    }
}
