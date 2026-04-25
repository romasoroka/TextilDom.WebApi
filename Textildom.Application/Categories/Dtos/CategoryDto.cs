namespace Textildom.Application.Categories.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubCategoryDto>? SubCategories { get; set; }
        public string IconKey { get; set; } = string.Empty;
    }

    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
