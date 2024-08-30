using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DeviceHubDbContext dbContext;
        private DbSet<T> dbSet;

        public GenericRepository(DeviceHubDbContext _dbContext)
        {
            dbContext = _dbContext;
            dbSet = dbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var word in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(word);
                }
            }
            return await query.ToListAsync();
        }


        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var word in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(word);
                }
            }
            return await query.FirstOrDefaultAsync() ;
        }

        public void  Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
