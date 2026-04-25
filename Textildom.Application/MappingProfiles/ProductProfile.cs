using AutoMapper;
using Textildom.Application.Products.Commands;
using Textildom.Application.Products.Dtos;
using Textildom.Application.Products.Dtos.Textildom.Application.Products.Dtos;
using Textildom.Domain.Models;

namespace Textildom.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapping для ProductImage
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductImageDto, ProductImage>();

            // Mapping для ProductVariant з усіма цінами
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<ProductVariantDto, ProductVariant>();

            // Entity → DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Name : null));

            CreateMap<Product, ProductShortDto>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Name : null));

            // Command → Entity
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.ProductImagesJson, opt => opt.Ignore())
                .ForMember(dest => dest.VariantsJson, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.ProductImagesJson, opt => opt.Ignore())
                .ForMember(dest => dest.VariantsJson, opt => opt.Ignore());
        }
    }
}

