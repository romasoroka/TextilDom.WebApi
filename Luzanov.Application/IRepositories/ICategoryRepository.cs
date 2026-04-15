using Luzanov.Domain.Models;

namespace Luzanov.Application.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByIdWithSubCategoriesAsync(int id);
        Task<IEnumerable<Category>> GetAllWithSubCategoriesAsync();
        Task<Category?> GetByNameAsync(string name);
    }
}
