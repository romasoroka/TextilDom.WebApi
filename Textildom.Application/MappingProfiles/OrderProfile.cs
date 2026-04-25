using AutoMapper;
using Textildom.Application.Orders.Commands;
using Textildom.Application.Orders.Dtos;
using Textildom.Domain.Models;

namespace Textildom.Application.MappingProfiles
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
