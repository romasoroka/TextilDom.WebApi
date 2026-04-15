using AutoMapper;
using Luzanov.Application.IRepositories;
using Luzanov.Application.Products.Commands;
using Luzanov.Application.Products.Dtos;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; 
using Azure.Storage.Blobs; 
using Azure.Storage.Blobs.Models; 

namespace Luzanov.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly string _azureConnectionString; 
        private readonly string _containerName; 

        public ProductService(IProductRepository productRepo, IMapper mapper, IConfiguration configuration)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _azureConnectionString = configuration.GetConnectionString("AzureBlobStorage")
                ?? throw new ArgumentNullException("Azure Connection String is missing");
            _containerName = configuration["AzureStorageConfig:ContainerName"] ?? "luzanov";
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

        public async Task<IEnumerable<ProductShortDto>> GetBySubCategoryIdAsync(int subCategoryId)
        {
            var products = await _productRepo.GetBySubCategoryIdAsync(subCategoryId);
            return _mapper.Map<IEnumerable<ProductShortDto>>(products);
        }

        public async Task<IEnumerable<ProductShortDto>> GetSpecialOffersAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var specialOffers = products.Where(p => p.IsSpecialOffer);
            return _mapper.Map<IEnumerable<ProductShortDto>>(specialOffers);
        }

        public async Task<IEnumerable<ProductShortDto>> GetTopProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var topProducts = products.Where(p => p.IsTop);
            return _mapper.Map<IEnumerable<ProductShortDto>>(topProducts);
        }

        public async Task<ProductDto> CreateAsync(CreateProductCommand command)
        {
            if (command.IsSpecialOffer)
            {
                await RemoveSpecialOfferFromAllProductsAsync();
            }

            var product = _mapper.Map<Product>(command);

            await _productRepo.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }
        
        public async Task<bool> AddImagesAsync(int productId, List<IFormFile> images, List<string>? colours = null, List<bool>? isMainFlags = null)
        {
            if (images == null || images.Count == 0)
                return false;

            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return false;

            var savedImageUrls = await SaveImagesAsync(images);
            if (savedImageUrls == null || savedImageUrls.Count == 0)
                return false;

            var currentImages = product.ProductImages ?? new List<ProductImage>();

            for (int i = 0; i < savedImageUrls.Count; i++)
            {
                var productImage = new ProductImage
                {
                    Url = savedImageUrls[i],
                    Colour = colours != null && i < colours.Count ? colours[i] : null,
                    IsMain = isMainFlags != null && i < isMainFlags.Count && isMainFlags[i]
                };

                currentImages.Add(productImage);
            }

            product.ProductImages = currentImages;

            await _productRepo.UpdateAsync(product);

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductCommand command)
        {
            var existing = await _productRepo.GetByIdAsync(command.Id);
            if (existing == null) return false;

            if (command.IsSpecialOffer && !existing.IsSpecialOffer)
            {
                await RemoveSpecialOfferFromAllProductsAsync(command.Id);
            }

            _mapper.Map(command, existing);

            return await _productRepo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;

            if (product.ProductImages != null && product.ProductImages.Any())
            {
                var imageUrls = product.ProductImages.Select(img => img.Url).ToList();
                await DeleteImagesAsync(imageUrls);
            }

            return await _productRepo.RemoveAsync(product);
        }
        
        public async Task<bool> DeleteImageAsync(int productId, string imageUrl)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return false;

            var images = product.ProductImages?.ToList() ?? new List<ProductImage>();
            if (!images.Any())
                return false;

            imageUrl = imageUrl.TrimStart('/', '\\').Replace("\\", "/");

            var imageToRemove = images.FirstOrDefault(img =>
                string.Equals(img.Url.TrimStart('/', '\\').Replace("\\", "/"),
                              imageUrl,
                              StringComparison.OrdinalIgnoreCase));

            if (imageToRemove == null)
                return false;

            await DeleteImagesAsync(new List<string> { imageToRemove.Url });

            images.Remove(imageToRemove);
            product.ProductImages = images;
            await _productRepo.UpdateAsync(product);

            return true;
        }

        private async Task RemoveSpecialOfferFromAllProductsAsync(int? excludeProductId = null)
        {
            var allProducts = await _productRepo.GetAllAsync();
            var productsToUpdate = allProducts.Where(p => p.IsSpecialOffer && p.Id != excludeProductId);

            foreach (var product in productsToUpdate)
            {
                product.IsSpecialOffer = false;
                await _productRepo.UpdateAsync(product);
            }
        }

        private async Task<List<string>> SaveImagesAsync(List<IFormFile> files)
        {
            var urls = new List<string>();
            if (files == null || !files.Any()) return urls;

            var blobServiceClient = new BlobServiceClient(_azureConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            foreach (var file in files)
            {
                if (file == null || file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                urls.Add(blobClient.Uri.ToString());
            }

            return urls;
        }

        private async Task DeleteImagesAsync(List<string>? urls)
        {
            if (urls == null || !urls.Any()) return;

            var blobServiceClient = new BlobServiceClient(_azureConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            foreach (var url in urls)
            {
                if (string.IsNullOrWhiteSpace(url)) continue;

                try
                {
                    var uri = new Uri(url);
                    var fileName = Path.GetFileName(uri.LocalPath);
                    var blobClient = containerClient.GetBlobClient(fileName);
                    await blobClient.DeleteIfExistsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete blob: {url}. Error: {ex.Message}");
                }
            }
        }
    }
}
