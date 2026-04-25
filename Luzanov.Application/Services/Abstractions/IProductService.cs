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
        Task<ImportProductsResult> ImportFromExcelAsync(ImportProductsCommand command);
        Task<IEnumerable<ProductShortDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> SearchByNameAsync(string name);
        Task<IEnumerable<ProductShortDto>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ProductShortDto>> GetSpecialOffersAsync();
        Task<IEnumerable<ProductShortDto>> GetTopProductsAsync();
        Task<bool> UpdateAsync(UpdateProductCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
