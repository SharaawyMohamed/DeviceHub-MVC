using System;
using System.Collections;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Infrastructure.Data.Contexts;

namespace DevicesHub.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeviceHubDbContext _context;
        private readonly Hashtable _repositories;
        public UnitOfWork(DeviceHubDbContext _dbContext)
        {
            _context = _dbContext;
            _repositories=new Hashtable();
           
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var objectType = typeof(T).Name;
            if(!_repositories.ContainsKey(objectType))
            {
                _repositories.Add(objectType,new GenericRepository<T>(_context));
            }
            return (IGenericRepository<T>) (_repositories[objectType])!;
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }
}
