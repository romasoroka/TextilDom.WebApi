namespace Luzanov.Application.Products.Dtos
{
    /// <summary>
    /// DTO варіанту товару з цінами
    /// </summary>
    public class ProductVariantDto
    {
        /// <summary>
        /// Розмір товару
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Основна ціна (роздрібна)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Ціна від 10 штук (опціонально)
        /// </summary>
        public decimal? PriceFrom10 { get; set; }

        /// <summary>
        /// Ціна від 20 штук (опціонально)
        /// </summary>
        public decimal? PriceFrom20 { get; set; }

        /// <summary>
        /// Ціна від 50 штук (опціонально)
        /// </summary>
        public decimal? PriceFrom50 { get; set; }
    }
}
