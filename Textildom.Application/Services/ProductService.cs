using AutoMapper;
using ClosedXML.Excel;
using System.Globalization;
using System.Text.RegularExpressions;
using Textildom.Application.IRepositories;
using Textildom.Application.Products.Commands;
using Textildom.Application.Products.Dtos;
using Textildom.Application.Services.Abstractions;
using Textildom.Domain.Models;

namespace Textildom.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductShortDto>> GetAllAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductShortDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> SearchByNameAsync(string name)
        {
            var products = await _productRepo.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductShortDto>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepo.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductShortDto>>(products);
        }

        public async Task<IEnumerable<ProductShortDto>> GetSpecialOffersAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductShortDto>>(products.Where(p => p.IsSpecialOffer));
        }

        public async Task<IEnumerable<ProductShortDto>> GetTopProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductShortDto>>(products.Where(p => p.IsTop));
        }

        public async Task<ImportProductsResult> ImportFromExcelAsync(ImportProductsCommand command)
        {
            var result = new ImportProductsResult();
            using var stream = command.File.OpenReadStream();
            using var workbook = new XLWorkbook(stream);

            foreach (var sheet in workbook.Worksheets)
            {
                Console.WriteLine($"Processing sheet: {sheet.Name}");

                var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;
                if (lastRow < 2) continue;

                var headerRow = sheet.Row(1);
                var headers = headerRow.Cells()
                    .Select((cell, idx) => new { Name = cell.GetString().Trim(), Index = idx + 1 })
                    .ToDictionary(h => h.Name, h => h.Index, StringComparer.OrdinalIgnoreCase);

                int Col(string name) => headers.TryGetValue(name, out var idx) ? idx : -1;
                string GetCell(IXLRow row, int colIndex) => colIndex > 0 ? row.Cell(colIndex).GetString().Trim() : "";

                for (int i = 2; i <= lastRow; i++)
                {
                    var row = sheet.Row(i);
                    try
                    {
                        var name = GetCell(row, Col("Назва (укр)"));
                        if (string.IsNullOrWhiteSpace(name)) continue;

                        var imagesRaw = GetCell(row, Col("Изображения"));
                        var images = imagesRaw
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select((url, idx) => new ProductImage
                            {
                                Url = url.Trim(),
                                IsMain = idx == 0,
                                Colour = null
                            }).ToList();

                        var variant = new ProductVariant
                        {
                            Width = 0,
                            Height = 0,
                            Colour = null,
                            Price = ParseDecimal(GetCell(row, Col("Цена"))),
                            OldPrice = ParseNullableDecimal(GetCell(row, Col("Старая цена"))),
                            InStock = GetCell(row, Col("Наличие")).Contains("наличии", StringComparison.OrdinalIgnoreCase),
                            Quantity = null,
                        };

                        var product = new Product
                        {
                            Name = name,
                            Description = GetCell(row, Col("Полное описание (UA)")),
                            Manufacturer = GetCell(row, Col("Производитель")),
                            Variants = new List<ProductVariant> { variant },
                            ProductImages = images,
                            IsSpecialOffer = false,
                            IsTop = false,
                        };

                        await _productRepo.AddAsync(product);
                        result.Created++;
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Аркуш '{sheet.Name}', рядок {i}: {ex.Message}");
                        result.Skipped++;
                    }
                }
            }

            return result;
        }
        private static decimal ParseDecimal(string value)
            => decimal.TryParse(value.Replace(",", "."), NumberStyles.Any,
                CultureInfo.InvariantCulture, out var r) ? r : 0;

        private static decimal? ParseNullableDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "0") return null;
            return decimal.TryParse(value.Replace(",", "."), NumberStyles.Any,
                CultureInfo.InvariantCulture, out var r) ? r : null;
        }

        private static int? ParseNullableInt(string value)
            => int.TryParse(value, out var r) ? r : null;

        public async Task<bool> UpdateAsync(UpdateProductCommand command)
        {
            var existing = await _productRepo.GetByIdAsync(command.Id);
            if (existing == null) return false;

            if (command.IsSpecialOffer && !existing.IsSpecialOffer)
                await RemoveSpecialOfferFromAllProductsAsync(command.Id);

            _mapper.Map(command, existing);
            return await _productRepo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;
            return await _productRepo.RemoveAsync(product);
        }

        private async Task RemoveSpecialOfferFromAllProductsAsync(int? excludeProductId = null)
        {
            var allProducts = await _productRepo.GetAllAsync();
            foreach (var product in allProducts.Where(p => p.IsSpecialOffer && p.Id != excludeProductId))
            {
                product.IsSpecialOffer = false;
                await _productRepo.UpdateAsync(product);
            }
        }

    }
}