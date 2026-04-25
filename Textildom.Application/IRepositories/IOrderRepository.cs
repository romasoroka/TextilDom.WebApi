using Textildom.Domain.Models;

namespace Textildom.Application.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
    }
}
