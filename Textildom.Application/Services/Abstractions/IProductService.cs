using Textildom.Application.Products.Commands;
using Textildom.Application.Products.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Services.Abstractions
{
    public interface IProductService
    {
        Task<ImportProductsResult> ImportFromExcelAsync(ImportProductsCommand command);
        Task<byte[]> ExportToExcelAsync();
        Task<IEnumerable<ProductShortDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> SearchByNameAsync(string name);
        Task<ProductDto> CreateManualAsync(CreateProductManualCommand command);
        Task<IEnumerable<ProductShortDto>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ProductShortDto>> GetSpecialOffersAsync();
        Task<IEnumerable<ProductShortDto>> GetTopProductsAsync();
        Task<bool> UpdateAsync(UpdateProductCommand command);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteManyAsync(List<int> ids);
    }
}
