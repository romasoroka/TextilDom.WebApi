using Luzanov.Application.Users.Commands;
using Luzanov.Application.Users.Dtos;
using Luzanov.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Services.Abstractions
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateUserAsync(CreateUserCommand command);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordCommand command);
        Task<bool> DeleteAsync(int id);
    }
}
