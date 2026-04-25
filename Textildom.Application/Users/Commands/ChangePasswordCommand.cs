using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Users.Commands
{
    public class ChangePasswordCommand
    {
        public string UserName { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
