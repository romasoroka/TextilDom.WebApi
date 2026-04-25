using AutoMapper;
using Textildom.Application.Categories.Commands;
using Textildom.Application.Categories.Dtos;
using Textildom.Domain.Models;

namespace Textildom.Application.MappingProfiles
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
