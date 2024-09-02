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
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null);
        public Task<CategoryVM>? GetFirstCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null);

        Task<int> UpdateCategoryAsync(CategoryVM entity);
        Task<int> AddCategoryAsync(CategoryVM entity);
        Task<int> RemoveCategoryAsync(CategoryVM entity);
    }
}
