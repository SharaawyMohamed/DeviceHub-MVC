using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>>? GetAllAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);
        Task<T>? GetFirstAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
