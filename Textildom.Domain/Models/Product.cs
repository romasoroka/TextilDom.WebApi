using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Textildom.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Material { get; set; }
        public string? Colour { get; set; }
        public string? Features { get; set; }
        public string? CareInstructions { get; set; }
        public string? Fastening { get; set; }
        public string? ProductType { get; set; }
        public string? Purpose { get; set; }
        public string? Decoration { get; set; }

        public bool IsSpecialOffer { get; set; } = false;
        public bool IsTop { get; set; } = false;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public string VariantsJson { get; set; } = string.Empty;
        public string ProductImagesJson { get; set; } = string.Empty;

        [NotMapped]
        public List<ProductVariant> Variants
        {
            get => string.IsNullOrEmpty(VariantsJson)
                ? new List<ProductVariant>()
                : JsonSerializer.Deserialize<List<ProductVariant>>(VariantsJson)!;
            set => VariantsJson = JsonSerializer.Serialize(value ?? new List<ProductVariant>());
        }

        [NotMapped]
        public List<ProductImage> ProductImages
        {
            get => string.IsNullOrEmpty(ProductImagesJson)
                ? new List<ProductImage>()
                : JsonSerializer.Deserialize<List<ProductImage>>(ProductImagesJson)!;
            set => ProductImagesJson = JsonSerializer.Serialize(value ?? new List<ProductImage>());
        }
    }

    public class ProductVariant
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string? Colour { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public bool InStock { get; set; }
        public string? Quantity { get; set; } 
    }

    public class ProductImage
    {
        public string Url { get; set; } = string.Empty;
        public string? Colour { get; set; }
        public bool IsMain { get; set; }
    }
}
