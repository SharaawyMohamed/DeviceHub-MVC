using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Infrastructure.Data.Contexts;

namespace DevicesHub.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DeviceHubDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}
