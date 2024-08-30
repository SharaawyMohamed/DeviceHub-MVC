using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Services
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCart>> GetAllShoppingCartsAsync(Expression<Func<ShoppingCart, bool>>? predicate = null, string? IncludeWord = null);
        Task<ShoppingCart> GetFirstShoppingCartAsync(Expression<Func<ShoppingCart, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateShoppingCartAsync(ShoppingCart entity);
        Task<int> AddShoppingCartAsync(ShoppingCart entity);
        Task<int> RemoveShoppingCartAsync(ShoppingCart entity);
        Task<int> RemoveRangeOfShoppingCartsAsync(IEnumerable<ShoppingCart> entities);

        public Task<int> IncreaseCountAsync(ShoppingCart shoppingCart, int count);
        public Task<int> DecreaseCountAsync(ShoppingCart shoppingCart, int count);
    }
}
