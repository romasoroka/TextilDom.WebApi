using Luzanov.Application.SubCategories.Commands;
using Luzanov.Application.SubCategories.Dtos;

namespace Luzanov.Application.Services.Abstractions
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDto>> GetAllAsync();
        Task<IEnumerable<SubCategoryDto>> GetByCategoryIdAsync(int categoryId);
        Task<SubCategoryDto?> GetByIdAsync(int id);
        Task<SubCategoryDto> CreateAsync(CreateSubCategoryCommand command);
        Task<bool> UpdateAsync(UpdateSubCategoryCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
