using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Services
{
    public interface IOrderHeaderService
    {

        Task<IEnumerable<OrderHeader>> GetAllOrderHeadersAsync(Expression<Func<OrderHeader, bool>>? predicate = null, string? IncludeWord = null);
        Task<OrderHeader> GetFirstOrderHeadersAsync(Expression<Func<OrderHeader, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateOrderHeaderAsync(OrderHeader entity);
        Task<int> AddOrderHeaderAsync(OrderHeader entity);
        Task<int> RemoveOrderHeaderAsync(OrderHeader entity);
        Task<int> RemoveRangeOfOrdersHeadersAsync(IEnumerable<OrderHeader> entities);
        Task<int> UpdateOrderStatus(int Id, string OrderStatus, string PaymentStatus);

    }
}
