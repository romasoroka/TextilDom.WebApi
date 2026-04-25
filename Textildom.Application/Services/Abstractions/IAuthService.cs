using Textildom.Application.Login.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginCommand command);
    }
}
