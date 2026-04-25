using AutoMapper;
using Luzanov.Application.Categories.Commands;
using Luzanov.Application.Categories.Dtos;
using Luzanov.Application.IRepositories;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Models;

namespace Luzanov.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }


        public async Task<CategoryDto> CreateAsync(CreateCategoryCommand command)
        {
            var category = _mapper.Map<Category>(command);
            await _categoryRepo.AddAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateAsync(UpdateCategoryCommand command)
        {
            var existing = await _categoryRepo.GetByIdAsync(command.Id);
            if (existing == null) return false;

            _mapper.Map(command, existing);
            return await _categoryRepo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            return await _categoryRepo.RemoveAsync(category);
        }
    }
}
