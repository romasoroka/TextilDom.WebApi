namespace Luzanov.Application.Products.Dtos
{
    public class ProductImageDto
    {
        public string Url { get; set; } = string.Empty;
        public string? Colour { get; set; } // якщо null Ч це загальне фото, €кщо Ї назва Ч це фото конкретного кольору
        public bool IsMain { get; set; }
    }
}
