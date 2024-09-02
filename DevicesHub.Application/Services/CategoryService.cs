using AutoMapper;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using DevicesHub.Domain.ViewModels;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddCategoryAsync(CategoryVM entity)
        {
            var maped=_mapper.Map<Category>(entity);
            await _unitOfWork.Repository<Category>().AddAsync(maped);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CategoryVM>>? GetAllCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null)
        {
            var Categories = await (_unitOfWork.Repository<Category>().GetAllAsync(predicate, IncludeWord))!;
            var mapped= _mapper.Map<IEnumerable<CategoryVM>>(Categories);
            return mapped;
        }
        public async Task<CategoryVM>? GetFirstCategoryAsync(Expression<Func<Category, bool>>? predicate = null, string? IncludeWord = null)
        {
            var category=await _unitOfWork.Repository<Category>().GetFirstAsync(predicate, IncludeWord);
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task<int> RemoveCategoryAsync(CategoryVM entity)
        {
            var mapped = _mapper.Map<Category>(entity);
            _unitOfWork.Repository<Category>().Remove(mapped);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateCategoryAsync(CategoryVM entity)
        {
            var mapped = _mapper.Map<Category>(entity);
            _unitOfWork.Repository<Category>().Update(mapped);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
