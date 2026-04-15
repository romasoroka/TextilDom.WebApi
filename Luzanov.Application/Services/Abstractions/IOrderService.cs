using Luzanov.Application.Orders.Commands;
using Luzanov.Application.Orders.Dtos;

namespace Luzanov.Application.Services.Abstractions
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(CreateOrderCommand command);
        Task<bool> UpdateAsync(UpdateOrderCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
