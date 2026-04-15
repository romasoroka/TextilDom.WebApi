using Luzanov.Application.Products.Commands;
using Luzanov.Application.Products.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Services.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductShortDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> SearchByNameAsync(string name);
        Task<IEnumerable<ProductShortDto>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ProductShortDto>> GetBySubCategoryIdAsync(int subCategoryId);
        Task<IEnumerable<ProductShortDto>> GetSpecialOffersAsync();
        Task<IEnumerable<ProductShortDto>> GetTopProductsAsync();
        Task<ProductDto> CreateAsync(CreateProductCommand command);
        Task<bool> AddImagesAsync(int productId, List<IFormFile> images, List<string>? colours = null, List<bool>? isMainFlags = null);
        Task<bool> DeleteImageAsync(int productId, string imageUrl);
        Task<bool> UpdateAsync(UpdateProductCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
