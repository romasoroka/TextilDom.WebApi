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
            var sheet = workbook.Worksheet(1);
            var rows = sheet.RowsUsed().ToList();
            if (rows.Count < 2) return result;

            var headerRow = rows[0];
            var headers = headerRow.Cells()
                .Select((cell, idx) => new { Name = cell.GetString().Trim(), Index = idx + 1 })
                .ToDictionary(h => h.Name, h => h.Index);

            int Col(string name) => headers.TryGetValue(name, out var idx) ? idx : -1;
            string GetCell(IXLRow row, int colIndex) => colIndex > 0 ? row.Cell(colIndex).GetString().Trim() : "";

            // Групуємо по базовій назві (без розміру в кінці)
            var grouped = rows.Skip(1)
                .GroupBy(row => ExtractBaseName(GetCell(row, Col("Назва (укр)"))))
                .ToList();

            foreach (var group in grouped)
            {
                try
                {
                    var baseName = group.Key;
                    if (string.IsNullOrWhiteSpace(baseName)) continue;

                    var firstRow = group.First();

                    // Кожен рядок = окремий варіант
                    var variants = group.Select(row => new ProductVariant
                    {
                        Width = ParseDecimal(GetCell(row, Col("Ширина|24907"))),
                        Height = ParseDecimal(GetCell(row, Col("Высота|24906"))),
                        Colour = GetCell(row, Col("Цвет|24909")),
                        Price = ParseDecimal(GetCell(row, Col("Цена"))),
                        OldPrice = ParseNullableDecimal(GetCell(row, Col("Старая цена"))),
                        InStock = GetCell(row, Col("Наличие")).Contains("наличии", StringComparison.OrdinalIgnoreCase),
                        Quantity = GetCell(row, Col("Комплектация;UA|24482")),
                    }).ToList();

                    // Фото з першого рядка групи
                    var imagesRaw = GetCell(firstRow, Col("Изображения"));
                    var images = imagesRaw
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select((url, i) => new ProductImage
                        {
                            Url = url.Trim(),
                            IsMain = i == 0,
                            Colour = null
                        }).ToList();

                    var product = new Product
                    {
                        Name = baseName,
                        Description = GetCell(firstRow, Col("Полное описание (UA)")),
                        Manufacturer = GetCell(firstRow, Col("Производитель")),
                        Material = GetCell(firstRow, Col("Материал|24904")),
                        Colour = GetCell(firstRow, Col("Цвет|24909")),
                        Features = GetCell(firstRow, Col("Особенности|130872")),
                        CareInstructions = GetCell(firstRow, Col("Рекомендации по уходу|122426")),
                        Fastening = GetCell(firstRow, Col("Способ крепления|24905")),
                        ProductType = GetCell(firstRow, Col("Тип|24902")),
                        Purpose = GetCell(firstRow, Col("Назначение|244048")),
                        Decoration = GetCell(firstRow, Col("Декорирование|113211")),
                        Variants = variants,
                        ProductImages = images,
                        CategoryId = null,
                        IsSpecialOffer = false,
                        IsTop = false,
                    };

                    await _productRepo.AddAsync(product);
                    result.Created++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Група '{group.Key}': {ex.Message}");
                    result.Skipped++;
                }
            }

            return result;
        }

        private static string ExtractBaseName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return fullName;
            return Regex.Replace(fullName.Trim(), @"\s+\d+[хxХX]\d+\s*$", "").Trim();
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