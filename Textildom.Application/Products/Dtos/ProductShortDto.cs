using Textildom.Application.Products.Dtos.Textildom.Application.Products.Dtos;

namespace Textildom.Application.Products.Dtos
{
    public class ProductShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Colour { get; set; }
        public bool IsSpecialOffer { get; set; }
        public bool IsTop { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<ProductImageDto> ProductImages { get; set; } = new();
    }
}