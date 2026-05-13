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
        public async Task<ProductDto> CreateManualAsync(CreateProductManualCommand command)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description ?? "",
                Manufacturer = command.Manufacturer,
                Material = command.Material,
                Features = command.Features,
                CareInstructions = command.CareInstructions,
                Fastening = command.Fastening,
                ProductType = command.ProductType,
                Purpose = command.Purpose,
                Decoration = command.Decoration,
                IsSpecialOffer = command.IsSpecialOffer,
                IsTop = command.IsTop,
                CategoryId = command.CategoryId,
                Variants = command.Variants.Select(v => new ProductVariant
                {
                    Width = v.Width,
                    Height = v.Height,
                    Colour = v.Colour,
                    Price = v.Price,
                    OldPrice = v.OldPrice,
                    InStock = v.InStock,
                    Quantity = v.Quantity,
                }).ToList(),
                ProductImages = command.ImageUrls.Select((url, i) => new ProductImage
                {
                    Url = url,
                    IsMain = i == 0,
                    Colour = null
                }).ToList(),
            };

            await _productRepo.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
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
        public async Task<byte[]> ExportToExcelAsync()
        {
            var products = await _productRepo.GetAllAsync();

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Товари");

            // Заголовки ідентичні до імпорту
            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Назва (укр)";
            sheet.Cell(1, 3).Value = "Полное описание (UA)";
            sheet.Cell(1, 4).Value = "Производитель";
            sheet.Cell(1, 5).Value = "Наличие";
            sheet.Cell(1, 6).Value = "Цена";
            sheet.Cell(1, 7).Value = "Старая цена";
            sheet.Cell(1, 8).Value = "Изображения";

            var headerRow = sheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;

            int row = 2;
            foreach (var product in products)
            {
                var firstVariant = product.Variants.FirstOrDefault();
                var images = string.Join(";", product.ProductImages.Select(i => i.Url));

                sheet.Cell(row, 1).Value = product.Id;
                sheet.Cell(row, 2).Value = product.Name;
                sheet.Cell(row, 3).Value = product.Description;
                sheet.Cell(row, 4).Value = product.Manufacturer ?? "";
                sheet.Cell(row, 5).Value = firstVariant?.InStock == true ? "В наличии" : "Нет в наличии";
                sheet.Cell(row, 6).Value = firstVariant?.Price ?? 0;
                sheet.Cell(row, 7).Value = firstVariant?.OldPrice ?? 0;
                sheet.Cell(row, 8).Value = images;

                row++;
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public async Task<int> DeleteManyAsync(List<int> ids)
        {
            int deleted = 0;
            foreach (var id in ids)
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product == null) continue;
                await _productRepo.RemoveAsync(product);
                deleted++;
            }
            return deleted;
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

            // Зберігаємо поточні зображення перед маппінгом
            var images = existing.ProductImages.ToList();

            _mapper.Map(command, existing);

            if (command.RemoveImageUrls != null && command.RemoveImageUrls.Any())
            {
                images = images
                    .Where(img => !command.RemoveImageUrls
                        .Any(url => string.Equals(url.Trim(), img.Url.Trim(), StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            if (command.AddImageUrls != null && command.AddImageUrls.Any())
            {
                var existingUrls = images.Select(i => i.Url.Trim().ToLower()).ToHashSet();
                foreach (var url in command.AddImageUrls)
                {
                    if (string.IsNullOrWhiteSpace(url)) continue;
                    if (existingUrls.Contains(url.Trim().ToLower())) continue;

                    images.Add(new ProductImage
                    {
                        Url = url.Trim(),
                        IsMain = images.Count == 0,
                        Colour = null
                    });
                }
            }

            if (images.Any() && !images.Any(i => i.IsMain))
                images[0].IsMain = true;

            existing.ProductImages = images;

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