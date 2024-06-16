using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
