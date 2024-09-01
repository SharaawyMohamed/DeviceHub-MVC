using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using System.Linq.Expressions;

namespace DevicesHub.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddShoppingCartAsync(ShoppingCart entity)
        {
            await _unitOfWork.Repository<ShoppingCart>().AddAsync(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> DecreaseCountAsync(ShoppingCart shoppingCart, int count)
        {
            var ShoppingCart = await(_unitOfWork.Repository<ShoppingCart>().GetFirstAsync(x => x.Id == shoppingCart.Id))!;
            ShoppingCart.Count -= count;
            await _unitOfWork.CompleteAsync();
            return ShoppingCart.Count;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllShoppingCartsAsync(Expression<Func<ShoppingCart, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<ShoppingCart>().GetAllAsync(predicate, IncludeWord))!;
        }

        public async Task<ShoppingCart> GetFirstShoppingCartAsync(Expression<Func<ShoppingCart, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<ShoppingCart>().GetFirstAsync(predicate, IncludeWord))!;
        }

        public async Task<int> IncreaseCountAsync(ShoppingCart shoppingCart, int count)
        {
            var ShoppingCart = await (_unitOfWork.Repository<ShoppingCart>().GetFirstAsync(x => x.Id == shoppingCart.Id))!;
            ShoppingCart.Count += count;
            await _unitOfWork.CompleteAsync();
            return ShoppingCart.Count;
        }

        public async Task<int> RemoveRangeOfShoppingCartsAsync(IEnumerable<ShoppingCart> entities)
        {
             _unitOfWork.Repository<ShoppingCart>().RemoveRange(entities);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> RemoveShoppingCartAsync(ShoppingCart entity)
        {
             _unitOfWork.Repository<ShoppingCart>().Remove(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateShoppingCartAsync(ShoppingCart entity)
        {
             _unitOfWork.Repository<ShoppingCart>().Update(entity);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
