namespace Textildom.Domain.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Foreign key
        public int CategoryId { get; set; }
        
        // Navigation properties
        public Category Category { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
