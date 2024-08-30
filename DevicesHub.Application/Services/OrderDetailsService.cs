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
    public class OrderDetailsService : Domain.Services.IOrderDetailsService
    {
        private readonly Domain.Interfaces.IUnitOfWork _unitOfWork;

        public OrderDetailsService(Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddOrderDetailsAsync(OrderDetails entity)
        {
            await _unitOfWork.Repository<OrderDetails>().AddAsync(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<OrderDetails>>? GetAllOrdersDetailsAsync(Expression<Func<OrderDetails, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<OrderDetails>().GetAllAsync(predicate, IncludeWord))!;
        }

        public async Task<OrderDetails> GetFirstOrderDetailsAsync(Expression<Func<OrderDetails, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<OrderDetails>().GetFirstAsync(predicate, IncludeWord))!;
        }

        public async Task<int> RemoveOrderDetailsAsync(OrderDetails entity)
        {
            _unitOfWork.Repository<OrderDetails>().Remove(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> RemoveRangeOfOrdersDetailsAsync(IEnumerable<OrderDetails> entities)
        {
            _unitOfWork.Repository<OrderDetails>().RemoveRange(entities);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateOrderDetailsAsync(OrderDetails entity)
        {
            _unitOfWork.Repository<OrderDetails>().Update(entity);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
