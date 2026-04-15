namespace Luzanov.Application.Categories.Commands
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;
    }

    public class UpdateCategoryCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconKey { get; set; }= string.Empty;
    }
}
