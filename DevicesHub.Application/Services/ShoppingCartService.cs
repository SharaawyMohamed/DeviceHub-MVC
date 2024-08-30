using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> AddShoppingCartAsync(ShoppingCart entity)
        {
            throw new NotImplementedException();
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

        public Task<int> RemoveRangeOfShoppingCartsAsync(IEnumerable<ShoppingCart> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveShoppingCartAsync(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateShoppingCartAsync(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }
    }
}
