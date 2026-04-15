using Luzanov.Domain.Models;

namespace Luzanov.Application.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
    }
}
