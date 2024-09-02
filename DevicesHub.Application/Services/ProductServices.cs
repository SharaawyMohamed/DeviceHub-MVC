using AutoMapper;
using DevicesHub.Application.Settings;
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
    public class ProductServices : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddProductAsync(ProductVM entity)
        {

            entity.ImageName = DocumentSettings.UploadFile(entity.Image, "Products");

            await _unitOfWork.Repository<Product>().AddAsync(_mapper.Map<Product>(entity));
            return await _unitOfWork.CompleteAsync();

        }

        public async Task<IEnumerable<ProductVM>> GetAllProductsAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null)
        {
            var products= await (_unitOfWork.Repository<Product>().GetAllAsync(predicate, IncludeWord))!;
            var MappedProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(products);
            return MappedProduct;
        }

        public async Task<ProductVM> GetFirstProductAsync(Expression<Func<Product, bool>>? predicate = null, string? IncludeWord = null)
        {
            var product= await (_unitOfWork.Repository<Product>().GetFirstAsync(predicate, IncludeWord))!;
            return _mapper.Map<ProductVM>(product);
        }

        public async Task<int> RemoveProductAsync(ProductVM entity)
        {
            var mapped = _mapper.Map<Product>(entity);
             _unitOfWork.Repository<Product>().Remove(mapped);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateProductAsync(ProductVM entity)
        {
            if (entity.Image != null)
            {
                DocumentSettings.DeleteFile(entity.ImageName, "Products");
                entity.ImageName = DocumentSettings.UploadFile(entity.Image, "Products");
            }
            var mapped = _mapper.Map<Product>(entity);
            _unitOfWork.Repository<Product>().Update(mapped);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
