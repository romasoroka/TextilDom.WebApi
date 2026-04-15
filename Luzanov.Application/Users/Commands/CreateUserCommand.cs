using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Users.Commands
{
    /// <summary>
    /// Команда для створення нового користувача
    /// </summary>
    public class CreateUserCommand
    {
        /// <summary>
        /// Ім'я користувача
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        /// Пароль користувача
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// Чи є користувач адміністратором. За замовчуванням false (звичайний користувач)
        /// </summary>
        public bool IsAdmin { get; set; } = false;
    }
}
