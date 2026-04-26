using Textildom.Application.Products.Dtos.Textildom.Application.Products.Dtos;

namespace Textildom.Application.Products.Commands
{
    public class UpdateProductCommand
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
        public bool IsSpecialOffer { get; set; }
        public bool IsTop { get; set; }
        public int? CategoryId { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}