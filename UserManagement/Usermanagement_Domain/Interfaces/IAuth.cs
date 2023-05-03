using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usermanagement_Domain.Models;

namespace Usermanagement_Domain.Interfaces
{
    public interface IAuth
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
