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
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItemsJson, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore())
                .ForMember(dest => dest.MonoInvoiceId, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItemsJson, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

            // DTO ? Domain
            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}
