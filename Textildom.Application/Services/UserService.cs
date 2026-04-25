using AutoMapper;
using Textildom.Application.IRepositories;
using Textildom.Application.Services.Abstractions;
using Textildom.Application.Users.Commands;
using Textildom.Application.Users.Dtos;
using Textildom.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(CreateUserCommand command)
        {
            var user = _mapper.Map<User>(command);
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, command.Password);

            await _userRepo.AddAsync(user);
            return user;
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangePasswordCommand command)
        {
            var user = await _userRepo.GetByUsernameAsync(command.UserName);
            if (user == null) return false;

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, command.NewPassword);

            return await _userRepo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return false;

            return await _userRepo.RemoveAsync(user);
        }
    }
}
