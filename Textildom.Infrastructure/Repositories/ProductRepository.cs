using Textildom.Application.IRepositories;
using Textildom.Domain.Models;
using Textildom.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Textildom.Infrastructure.Repositories
{
    public class ProductRepository
        : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
        
        public async Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
        

        
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .ToListAsync();
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
