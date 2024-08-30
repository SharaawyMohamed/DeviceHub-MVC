using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Application.Services
{
    public class OrderHeaderService : IOrderHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DeviceHubDbContext _context;

        public OrderHeaderService(IUnitOfWork unitOfWork, DeviceHubDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<int> AddOrderHeaderAsync(OrderHeader entity)
        {
            await _unitOfWork.Repository<OrderHeader>().AddAsync(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<OrderHeader>>? GetAllOrderHeadersAsync(Expression<Func<OrderHeader, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<OrderHeader>().GetAllAsync(predicate, IncludeWord))!;
        }

        public async Task<OrderHeader>? GetFirstOrderHeadersAsync(Expression<Func<OrderHeader, bool>>? predicate = null, string? IncludeWord = null)
        {
            return await (_unitOfWork.Repository<OrderHeader>().GetFirstAsync(predicate, IncludeWord))!;
        }

        public async Task<int> RemoveOrderHeaderAsync(OrderHeader entity)
        {
            _unitOfWork.Repository<OrderHeader>().Remove(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> RemoveRangeOfOrdersHeadersAsync(IEnumerable<OrderHeader> entities)
        {
            _unitOfWork.Repository<OrderHeader>().RemoveRange(entities);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateOrderHeaderAsync(OrderHeader entity)
        {
            _unitOfWork.Repository<OrderHeader>().Update(entity);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateOrderStatus(int Id, string OrderStatus, string PaymentStatus)
        {
            var order = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == Id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus;
                order.PaymentDate = DateTime.Now;
                if (PaymentStatus != null)
                    order.PaymentStatus = PaymentStatus;
            }
            return await _unitOfWork.CompleteAsync();
        }
    }
}
