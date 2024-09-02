using DevicesHub.Domain.Models;
using DevicesHub.Domain.ViewModels;
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
        Task<IEnumerable<ProductVM>> GetAllProductsAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null);
        Task<ProductVM> GetFirstProductAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateProductAsync(ProductVM entity);
        Task<int> AddProductAsync(ProductVM entity);
        Task<int> RemoveProductAsync(ProductVM entity);
    }
}
