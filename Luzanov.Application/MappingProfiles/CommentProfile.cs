using AutoMapper;
using Luzanov.Application.Comments.Commands;
using Luzanov.Application.Comments.Dtos;
using Luzanov.Domain.Models;

namespace Luzanov.Application.MappingProfiles
{
    /// <summary>
    /// Профіль маппінгу для коментарів
    /// </summary>
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // Маппінг з сутності в DTO з назвою товару
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.ProductName, 
                          opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));

            // Маппінг з команди створення в сутність
            CreateMap<CreateCommentCommand, Comment>();
        }
    }
}
