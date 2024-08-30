using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null);
        Task<Product> GetFirstProductAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateProductAsync(Product entity);
        Task<int> AddProductAsync(Product entity);
        Task<int> RemoveProductAsync(Product entity);
        Task<int> RemoveRangeOfProductsAsync(IEnumerable<Product> entities);
    }
}
