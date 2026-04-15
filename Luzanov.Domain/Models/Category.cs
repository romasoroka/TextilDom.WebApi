namespace Luzanov.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
