using Luzanov.Application.Products.Dtos;
using Luzanov.Application.Products.Dtos.Luzanov.Application.Products.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Products.Commands
{
    public class CreateProductCommand
    {
        public string Name { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<ProductVariantDto> Variants { get; set; } = new();
        
        public bool IsSpecialOffer { get; set; } = false;
        public bool IsTop { get; set; } = false;
        
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
