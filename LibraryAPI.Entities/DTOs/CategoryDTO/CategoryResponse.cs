namespace LibraryAPI.Entities.DTOs.CategoryDTO
{
    public class CategoryResponse
    {
        public short CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string CategoryStatus { get; set; } = string.Empty;

        public List<string>? SubCategoryNamesAndIds { get; set; }
    }
}
