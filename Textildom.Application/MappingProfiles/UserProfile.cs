using AutoMapper;
using Textildom.Application.Users.Commands;
using Textildom.Application.Users.Dtos;
using Textildom.Domain.Constants;
using Textildom.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Textildom.Application.MappingProfiles
{
    /// <summary>
    /// Профіль маппінгу для користувачів
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => 
                    src.IsAdmin ? UserRoles.Admin : UserRoles.User));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
