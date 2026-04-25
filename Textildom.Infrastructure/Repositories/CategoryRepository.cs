using Textildom.Application.IRepositories;
using Textildom.Domain.Models;
using Textildom.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Textildom.Infrastructure.Repositories
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
