using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null);
        Task<Category> GetFirstCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateCategoryAsync(Category entity);
        Task<int> AddCategoryAsync(Category entity);
        Task<int> RemoveCategoryAsync(Category entity);
        Task<int> RemoveRangeOfCategoriesAsync(IEnumerable<Category> entities);
    }
}
