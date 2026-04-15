using Luzanov.Application.Login.Commands;
using Luzanov.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Luzanov.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var token = await _authService.LoginAsync(command);
            if (token == null)
                return Unauthorized(new { message = "Невірний логін або пароль" });

            return Ok(new { token });
        }
    }
}
