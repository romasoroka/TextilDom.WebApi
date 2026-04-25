using Textildom.Application.Products.Dtos.Textildom.Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Products.Dtos
{

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ProductVariantDto> Variants { get; set; } = new();

        public List<ProductImageDto> ProductImages { get; set; } = new();

        public bool IsSpecialOffer { get; set; }
        public bool IsTop { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
    }

}
