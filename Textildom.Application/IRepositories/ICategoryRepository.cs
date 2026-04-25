using Textildom.Domain.Models;

namespace Textildom.Application.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
    }
}
