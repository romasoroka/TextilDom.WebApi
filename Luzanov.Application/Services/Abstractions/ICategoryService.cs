using Luzanov.Application.Categories.Commands;
using Luzanov.Application.Categories.Dtos;

namespace Luzanov.Application.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<IEnumerable<CategoryDto>> GetAllWithSubCategoriesAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto?> GetByIdWithSubCategoriesAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryCommand command);
        Task<bool> UpdateAsync(UpdateCategoryCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
