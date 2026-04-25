using Textildom.Application.IRepositories;
using Textildom.Domain.Models;
using Textildom.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Textildom.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbSet
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
