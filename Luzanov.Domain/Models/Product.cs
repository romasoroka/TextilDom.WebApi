using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Luzanov.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string VariantsJson { get; set; } = string.Empty;
        public string ProductImagesJson { get; set; } = string.Empty;

        // Flags
        public bool IsSpecialOffer { get; set; } = false;
        public bool IsTop { get; set; } = false;

        // Foreign keys
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public SubCategory? SubCategory { get; set; }

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

    /// <summary>
    /// Варіант товару з ціною та оптовими цінами
    /// </summary>
    public class ProductVariant
    {
        public decimal Width { get; set; }   // Ширина|24907
        public decimal Height { get; set; }  // Висота|24906
        public string? Colour { get; set; }  // Цвет|24909
        public decimal Price { get; set; }   // Цена
        public decimal? OldPrice { get; set; } // Старая цена
        public decimal? PromoPrice { get; set; } // Цена промо
        public int Stock { get; set; }       // Остатки
    }

    public class ProductImage
    {
        public string Url { get; set; } = string.Empty;
        public string? Colour { get; set; }
        public bool IsMain { get; set; }
    }
}
