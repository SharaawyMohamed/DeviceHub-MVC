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
    internal class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(DeviceHubDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<int> DecreaseCountAsync(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public async Task<int> IncreaseCountAsync(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }
}
