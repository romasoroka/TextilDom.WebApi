using Luzanov.Application.IRepositories;
using Luzanov.Domain.Models;
using Luzanov.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Luzanov.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
