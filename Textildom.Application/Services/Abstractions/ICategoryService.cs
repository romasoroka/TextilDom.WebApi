using Textildom.Application.Categories.Commands;
using Textildom.Application.Categories.Dtos;

namespace Textildom.Application.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryCommand command);
        Task<bool> UpdateAsync(UpdateCategoryCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
