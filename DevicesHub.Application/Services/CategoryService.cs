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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly Domain.Interfaces.IUnitOfWork _unitOfWork;
        public CategoryService(Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddCategoryAsync(Category entity)
        {
            await _unitOfWork.Repository<Category>().AddAsync(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Category>>? GetAllCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<Category>().GetAllAsync(predicate, IncludeWord))!;
        }

        public async Task<Category>? GetFirstCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null)
        {

            return await (_unitOfWork.Repository<Category>().GetFirstAsync(predicate, IncludeWord))!;
        }

        public async Task<int> RemoveCategoryAsync(Category entity)
        {
            _unitOfWork.Repository<Category>().Remove(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> RemoveRangeOfCategoriesAsync(IEnumerable<Category> entities)
        {
            _unitOfWork.Repository<Category>().RemoveRange(entities);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateCategoryAsync(Category entity)
        {
            _unitOfWork.Repository<Category>().Update(entity);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
