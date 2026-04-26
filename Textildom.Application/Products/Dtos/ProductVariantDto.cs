namespace Textildom.Application.Products.Dtos
{
    namespace Textildom.Application.Products.Dtos
    {
        public class ProductVariantDto
        {
            public decimal Width { get; set; }
            public decimal Height { get; set; }
            public string? Colour { get; set; }
            public decimal Price { get; set; }
            public decimal? OldPrice { get; set; }
            public bool InStock { get; set; }
            public string? Quantity { get; set; }
        }
    }
}
