using Luzanov.Application.Categories.Commands;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzanov.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase 
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category =  await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound(new { message = "Category is not found" });

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.CreateAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _categoryService.UpdateAsync(command);
            if (!success)
                return NotFound(new { message = "Category is not found" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Category is not found" });

            return NoContent();
        }
    }
}
