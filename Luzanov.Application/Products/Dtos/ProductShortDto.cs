using Luzanov.Application.Products.Dtos.Luzanov.Application.Products.Dtos;
using System;
using System.Collections.Generic;

namespace Luzanov.Application.Products.Dtos
{
    public class ProductShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductVariantDto> Variants { get; set; } = new();
        public decimal Discount { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new();
        
        public bool IsSpecialOffer { get; set; }
        public bool IsTop { get; set; }
        
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
    }
}
