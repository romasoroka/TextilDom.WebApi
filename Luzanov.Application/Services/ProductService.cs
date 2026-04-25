using AutoMapper;
using ClosedXML.Excel;
using Luzanov.Application.IRepositories;
using Luzanov.Application.Products.Commands;
using Luzanov.Application.Products.Dtos;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Models;

namespace Luzanov.Application.Services
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
            var rows = sheet.RowsUsed().Skip(1).ToList(); // пропускаємо заголовок

            // Читаємо заголовки щоб знайти індекси колонок
            var headerRow = sheet.Row(1);
            var headers = headerRow.Cells()
                .Select((cell, idx) => new { Name = cell.GetString().Trim(), Index = idx + 1 })
                .ToDictionary(h => h.Name, h => h.Index);

            int Col(string name) => headers.TryGetValue(name, out var idx) ? idx : -1;

            // Групуємо рядки по базовій назві (колонка "Назва (укр)")
            // Базова назва — це назва без розміру в кінці (напр. "100х170")
            var grouped = rows
                .GroupBy(row => ExtractBaseName(row.Cell(Col("Назва (укр)")).GetString()))
                .ToList();

            var allProducts = await _productRepo.GetAllAsync();

            foreach (var group in grouped)
            {
                try
                {
                    var baseName = group.Key;
                    if (string.IsNullOrWhiteSpace(baseName)) continue;

                    var firstRow = group.First();

                    // Збираємо варіанти з усіх рядків групи
                    var variants = group.Select(row =>
                    {
                        var widthCol = Col("Ширина|24907");
                        var heightCol = Col("Висота|24906");
                        var colourCol = Col("Цвет|24909");
                        var priceCol = Col("Цена");
                        var oldPriceCol = Col("Старая цена");
                        var promoPriceCol = Col("Цена промо");
                        var stockCol = Col("Остатки");

                        return new ProductVariant
                        {
                            Width = widthCol > 0 ? ParseDecimal(row.Cell(widthCol).GetString()) : 0,
                            Height = heightCol > 0 ? ParseDecimal(row.Cell(heightCol).GetString()) : 0,
                            Colour = colourCol > 0 ? row.Cell(colourCol).GetString().Trim() : null,
                            Price = priceCol > 0 ? ParseDecimal(row.Cell(priceCol).GetString()) : 0,
                            OldPrice = oldPriceCol > 0 ? ParseNullableDecimal(row.Cell(oldPriceCol).GetString()) : null,
                            PromoPrice = promoPriceCol > 0 ? ParseNullableDecimal(row.Cell(promoPriceCol).GetString()) : null,
                            Stock = stockCol > 0 ? (int)ParseDecimal(row.Cell(stockCol).GetString()) : 0,
                        };
                    }).ToList();

                    // Збираємо фото — беремо з першого рядка, розділяємо по ";"
                    var imagesCol = Col("Изображения");
                    var imageUrls = imagesCol > 0
                        ? firstRow.Cell(imagesCol).GetString()
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select((url, i) => new ProductImage
                            {
                                Url = url.Trim(),
                                IsMain = i == 0,
                                Colour = null
                            }).ToList()
                        : new List<ProductImage>();

                    var descCol = Col("Полное описание (UA)");
                    var description = descCol > 0 ? firstRow.Cell(descCol).GetString().Trim() : "";

                    // Перевіряємо чи вже існує продукт з такою назвою
                    var existing = allProducts.FirstOrDefault(p =>
                        string.Equals(p.Name, baseName, StringComparison.OrdinalIgnoreCase));

                    if (existing != null)
                    {
                        // Оновлюємо варіанти та фото
                        existing.Variants = variants;
                        existing.ProductImages = imageUrls;
                        existing.Description = description;
                        await _productRepo.UpdateAsync(existing);
                        result.Updated++;
                    }
                    else
                    {
                        var product = new Product
                        {
                            Name = baseName,
                            Description = description,
                            Variants = variants,
                            ProductImages = imageUrls,
                            IsSpecialOffer = false,
                            IsTop = false,
                        };
                        await _productRepo.AddAsync(product);
                        result.Created++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Помилка для '{group.Key}': {ex.Message}");
                    result.Skipped++;
                }
            }

            return result;
        }

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

        // "Комплект Штор Льон Блекаут Бежевий 2 шт 100х170" → "Комплект Штор Льон Блекаут Бежевий 2 шт"
        private static string ExtractBaseName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return fullName;
            // Видаляємо розмір у форматі "100х170" або "100x170" з кінця рядка
            return System.Text.RegularExpressions.Regex
                .Replace(fullName.Trim(), @"\s+\d+[хxХX]\d+\s*$", "")
                .Trim();
        }

        private static decimal ParseDecimal(string value)
            => decimal.TryParse(value.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : 0;

        private static decimal? ParseNullableDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "0") return null;
            return decimal.TryParse(value.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : null;
        }
    }
}