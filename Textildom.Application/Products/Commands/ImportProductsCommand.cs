using Microsoft.AspNetCore.Http;

namespace Textildom.Application.Products.Commands
{
    public class ImportProductsCommand
    {
        public IFormFile File { get; set; } = null!;
    }
}
