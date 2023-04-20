using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usermanagement_Domain.DTOs;
using Usermanagement_Domain.Models;

namespace Usermanagement_Domain.Interfaces
{
    public interface IUser: IRepository<User>
    {
        Task<List<User>> GetFilteredUsersAsync(UserFilter filter);
    }
}
