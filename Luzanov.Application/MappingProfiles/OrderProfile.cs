using AutoMapper;
using Luzanov.Application.Orders.Commands;
using Luzanov.Application.Orders.Dtos;
using Luzanov.Domain.Models;

namespace Luzanov.Application.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Entity ? DTO
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>();

            // Command ? Entity
            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItemsJson, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.OrderStatus, opt => opt.Ignore());

            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItemsJson, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

            // DTO ? Domain
            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}
