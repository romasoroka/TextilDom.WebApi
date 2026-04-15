using AutoMapper;
using Luzanov.Application.Categories.Commands;
using Luzanov.Application.Categories.Dtos;
using Luzanov.Domain.Models;

namespace Luzanov.Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<CreateCategoryCommand, Category>();

            CreateMap<UpdateCategoryCommand, Category>();

            CreateMap<SubCategory, Categories.Dtos.SubCategoryDto>();
        }
    }
}
