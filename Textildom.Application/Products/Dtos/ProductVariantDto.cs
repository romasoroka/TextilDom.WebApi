namespace Textildom.Application.Products.Dtos
{
    /// <summary>
    /// DTO варіанту товару з цінами
    /// </summary>
    namespace Textildom.Application.Products.Dtos
    {
        public class ProductVariantDto
        {
            public decimal Width { get; set; }
            public decimal Height { get; set; }
            public string? Colour { get; set; }
            public decimal Price { get; set; }
            public decimal? OldPrice { get; set; }
            public decimal? PromoPrice { get; set; }
            public int Stock { get; set; }
        }
    }
}
