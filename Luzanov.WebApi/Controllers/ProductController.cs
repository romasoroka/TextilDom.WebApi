using Luzanov.Application.Products.Commands;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzanov.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (page < 1)
                return BadRequest("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100");

            var allProducts = await _service.GetAllAsync();

            // Фільтрація за діапазоном цін
            var filteredProducts = FilterByPriceRange(allProducts, minPrice, maxPrice);

            // Пагінація
            var pagedResult = ApplyPagination(filteredProducts, page, pageSize);

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0");

            var product = await _service.GetByIdAsync(id);
            return product != null ? Ok(product) : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name parameter is required");

            var products = await _service.SearchByNameAsync(name);

            return Ok(products);
        }

        [HttpGet("special-offers")]
        public async Task<IActionResult> GetSpecialOffers()
        {
            var products = await _service.GetSpecialOffersAsync();
            return Ok(products);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts()
        {
            var products = await _service.GetTopProductsAsync();
            return Ok(products);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(
            int categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (categoryId <= 0)
                return BadRequest("Category ID must be greater than 0");

            if (page < 1)
                return BadRequest("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100");

            var allProducts = await _service.GetByCategoryIdAsync(categoryId);

            // Фільтрація за діапазоном цін
            var filteredProducts = FilterByPriceRange(allProducts, minPrice, maxPrice);

            // Пагінація
            var pagedResult = ApplyPagination(filteredProducts, page, pageSize);

            return Ok(pagedResult);
        }

        [HttpGet("subcategory/{subCategoryId}")]
        public async Task<IActionResult> GetBySubCategory(
            int subCategoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (subCategoryId <= 0)
                return BadRequest("SubCategory ID must be greater than 0");

            if (page < 1)
                return BadRequest("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("Page size must be between 1 and 100");

            var allProducts = await _service.GetBySubCategoryIdAsync(subCategoryId);

            // Фільтрація за діапазоном цін
            var filteredProducts = FilterByPriceRange(allProducts, minPrice, maxPrice);

            // Пагінація
            var pagedResult = ApplyPagination(filteredProducts, page, pageSize);

            return Ok(pagedResult);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _service.CreateAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpPost("{productId:int}/images")]
        public async Task<IActionResult> UploadImages(
            int productId, 
            [FromForm] List<IFormFile> images,
            [FromForm] List<string>? colours = null,
            [FromForm] List<bool>? isMainFlags = null)
        {
            if (images == null || images.Count == 0)
                return BadRequest("No images uploaded.");

            if (productId <= 0)
                return BadRequest("Id must be greater than 0");

            await _service.AddImagesAsync(productId, images, colours, isMainFlags);
            return Ok(new { Message = "Images uploaded successfully." });
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpDelete("{productId:int}/images")]
        public async Task<IActionResult> DeleteImage(int productId, [FromQuery] string imageUrl)
        {
            if (productId <= 0)
                return BadRequest("Id must be greater than 0");

            if (string.IsNullOrWhiteSpace(imageUrl))
                return BadRequest("Image URL must be provided.");

            var result = await _service.DeleteImageAsync(productId, imageUrl);
            if (!result)
                return NotFound("Product or image not found.");

            return Ok(new { Message = "Image deleted successfully." });
        }

        [HttpPut]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.UpdateAsync(command);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0");

            var result = await _service.DeleteAsync(id);

            return result ? NoContent() : NotFound();
        }

        // Helper методи для пагінації та фільтрації
        private IEnumerable<Application.Products.Dtos.ProductShortDto> FilterByPriceRange(
            IEnumerable<Application.Products.Dtos.ProductShortDto> products,
            decimal? minPrice,
            decimal? maxPrice)
        {
            if (minPrice == null && maxPrice == null)
                return products;

            return products.Where(p =>
            {
                // Вибираємо найменшу ціну з масиву варіантів
                if (p.Variants == null || !p.Variants.Any())
                    return true;

                var minProductPrice = p.Variants.Min(v => v.Price);

                if (minPrice.HasValue && minProductPrice < minPrice.Value)
                    return false;

                if (maxPrice.HasValue && minProductPrice > maxPrice.Value)
                    return false;

                return true;
            });
        }

        private object ApplyPagination(
            IEnumerable<Application.Products.Dtos.ProductShortDto> products,
            int page,
            int pageSize)
        {
            var productsList = products.ToList();
            var totalItems = productsList.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedProducts = productsList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new
            {
                Items = pagedProducts,
                Pagination = new
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    HasPrevious = page > 1,
                    HasNext = page < totalPages
                }
            };
        }
    }
}
