using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Usermanagement_Domain.Models;

namespace Usermanagement_Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> propertyName);   
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> propertyName);
        Task DeleteAllAsync();
        Task SaveAsync();
    }
}
