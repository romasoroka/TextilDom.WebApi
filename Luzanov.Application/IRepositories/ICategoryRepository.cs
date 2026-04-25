using Luzanov.Domain.Models;

namespace Luzanov.Application.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
    }
}
