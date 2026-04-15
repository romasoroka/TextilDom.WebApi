namespace Luzanov.Application.SubCategories.Commands
{
    public class CreateSubCategoryCommand
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class UpdateSubCategoryCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
