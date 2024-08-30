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
    public class ProductServices : IProductService
    {
        private readonly Domain.Interfaces.IUnitOfWork _unitOfWork;

        public ProductServices(Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProductAsync(Product entity)
        {
            await _unitOfWork.Repository<Product>().AddAsync(entity);
            return await _unitOfWork.CompleteAsync();

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<Product>().GetAllAsync(predicate, IncludeWord))!;
        }

        public async Task<Product> GetFirstProductAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<Product>().GetFirstAsync(predicate, IncludeWord))!;
        }

        public async Task<int> RemoveProductAsync(Product entity)
        {
             _unitOfWork.Repository<Product>().Remove(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> RemoveRangeOfProductsAsync(IEnumerable<Product> entities)
        {
            _unitOfWork.Repository<Product>().RemoveRange(entities);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateProductAsync(Product entity)
        {
            _unitOfWork.Repository<Product>().Update(entity);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
