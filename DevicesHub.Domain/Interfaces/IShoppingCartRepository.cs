using DevicesHub.Domain.Models;

namespace DevicesHub.Domain.Interfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        public Task<int> IncreaseCountAsync(ShoppingCart shoppingCart, int count);
        public Task<int> DecreaseCountAsync(ShoppingCart shoppingCart, int count);
    }
}
