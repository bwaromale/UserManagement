using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usermanagement_Domain.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public ApiResponse Response{ get; set; }
    }
}
