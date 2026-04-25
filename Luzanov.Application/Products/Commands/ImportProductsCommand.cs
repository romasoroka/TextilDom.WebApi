using Microsoft.AspNetCore.Http;

namespace Luzanov.Application.Products.Commands
{
    public class ImportProductsCommand
    {
        public IFormFile File { get; set; } = null!;
    }
}
