using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Textildom.Application.Products.Dtos.Textildom.Application.Products.Dtos;

namespace Textildom.Application.Products.Commands
{
    public class CreateProductManualCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public string? Material { get; set; }
        public string? Features { get; set; }
        public string? CareInstructions { get; set; }
        public string? Fastening { get; set; }
        public string? ProductType { get; set; }
        public string? Purpose { get; set; }
        public string? Decoration { get; set; }
        public bool IsSpecialOffer { get; set; } = false;
        public bool IsTop { get; set; } = false;
        public int? CategoryId { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new(); 
    }
}
