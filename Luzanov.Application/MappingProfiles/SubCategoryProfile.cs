using AutoMapper;
using Luzanov.Application.SubCategories.Commands;
using Luzanov.Application.SubCategories.Dtos;
using Luzanov.Domain.Models;

namespace Luzanov.Application.MappingProfiles
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<CreateSubCategoryCommand, SubCategory>();

            CreateMap<UpdateSubCategoryCommand, SubCategory>();
        }
    }
}
