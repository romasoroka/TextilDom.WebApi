using Luzanov.Application.Services.Abstractions;
using Luzanov.Application.Users.Commands;
using Luzanov.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzanov.WebApi.Controllers
{
    /// <summary>
    /// Контролер для роботи з користувачами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Отримати всіх користувачів (тільки для адміністраторів)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Get()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Створити нового користувача (тільки для адміністраторів)
        /// Можна вказати роль користувача (Admin або User)
        /// </summary>
        /// <param name="user">Дані нового користувача</param>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateUserAsync(user);
                return Ok(new 
                { 
                    id = created.Id,
                    username = created.Username,
                    role = created.Role,
                    message = "Користувача успішно створено"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Змінити пароль користувача
        /// </summary>
        /// <param name="command">Дані для зміни пароля</param>
        [HttpPost("change-password")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(new 
                { 
                    message = "Помилка валідації",
                    errors = errors
                });
            }

            try
            {
                var result = await _service.ChangeUserPasswordAsync(command);

                if (!result)
                    return NotFound(new { message = $"Користувача з ім'ям '{command.UserName}' не знайдено" });

                return Ok(new { message = "Пароль успішно змінено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Видалити користувача (тільки для адміністраторів)
        /// </summary>
        /// <param name="id">ID користувача</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Id має бути більше 0" });

            var result = await _service.DeleteAsync(id);
            
            if (!result)
                return NotFound(new { message = "Користувача не знайдено" });

            return Ok(new { message = "Користувача успішно видалено" });
        }
    }
}
