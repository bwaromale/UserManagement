using Microsoft.EntityFrameworkCore;
using Usermanagement_Domain.DTOs;
using Usermanagement_Domain.Interfaces;
using Usermanagement_Domain.Models;

namespace UserManagement_DataAccess.InterfacesImplementation
{
    public class UserRepository : Repository<User>, IUser
    {
        private readonly UserManagementContext _context;
        private readonly ICaching _cacheService;
        private DbSet<User> dbSet;

        public UserRepository(UserManagementContext context, ICaching cacheService) : base(context, cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            dbSet = _context.Users;
        }
       

        public async Task<List<User>> GetFilteredUsersAsync(UserFilter filter)
        {
            var query = dbSet.AsQueryable();
            if (filter.Age > 0)
            {
                query = query.Where(u => filter.Age > 0);
            }

            if (!string.IsNullOrEmpty(filter.Gender))
            {
                query = query.Where(u => u.Gender == filter.Gender);
            }

            if (!string.IsNullOrEmpty(filter.MaritalStatus))
            {
                query = query.Where(u=> u.MaritalStatus == filter.MaritalStatus);
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(u => u.Address == filter.Address);
            }

            if (!string.IsNullOrEmpty(filter.City))
            {
                query = query.Where(u => u.City == filter.City);
            }

            if (!string.IsNullOrEmpty(filter.State))
            {
                query = query.Where(u => u.State == filter.State);
            }

            if (!string.IsNullOrEmpty(filter.Country))
            {
                query = query.Where(u => u.Country == filter.Country);
            }
            return await query.ToListAsync();
        }
    }
}
