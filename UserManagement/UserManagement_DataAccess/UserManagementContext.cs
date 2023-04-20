using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usermanagement_Domain.Models;

namespace UserManagement_DataAccess
{
    public class UserManagementContext : DbContext
    {
        public UserManagementContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
