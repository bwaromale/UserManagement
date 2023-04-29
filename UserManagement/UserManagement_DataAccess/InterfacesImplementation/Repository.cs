using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Usermanagement_Domain.Interfaces;

namespace UserManagement_DataAccess.InterfacesImplementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly UserManagementContext _context;
        private readonly ICaching _cacheService;
        private DbSet<T> dbSet;

        public Repository(UserManagementContext context, ICaching cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> propertyName)
        {
            T entity = await dbSet.FirstOrDefaultAsync(propertyName);
            if(entity != null)
            {
                dbSet.Remove(entity);
                await SaveAsync();
            }
        }
        public async Task DeleteAllAsync()
        {
            var all = await GetAllAsync();
            dbSet.RemoveRange(all);
            await SaveAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var cacheResult = _cacheService.GetData<IEnumerable<T>>("key");
            if(cacheResult != null)
            {
                return cacheResult;
            }
            cacheResult = await dbSet.ToListAsync();
            var expiryTime = DateTimeOffset.Now.AddSeconds(50);
            _cacheService.SetData<IEnumerable<T>>("key", cacheResult, expiryTime);
            return cacheResult;
            //return await dbSet.ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> propertyName)
        {
            return await dbSet.FirstOrDefaultAsync(propertyName);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }    
    }
}
