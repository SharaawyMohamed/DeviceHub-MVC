using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Services
{
    public interface IOrderDetailsService
    {
        Task<IEnumerable<OrderDetails>> GetAllOrdersDetailsAsync(Expression<Func<OrderDetails, bool>>? predicate = null, string? IncludeWord = null);
        Task<OrderDetails> GetFirstOrderDetailsAsync(Expression<Func<OrderDetails, bool>>? predicate = null, string? IncludeWord = null);
        Task<int> UpdateOrderDetailsAsync(OrderDetails entity);
        Task<int> AddOrderDetailsAsync(OrderDetails entity);
        Task<int> RemoveOrderDetailsAsync(OrderDetails entity);
        Task<int> RemoveRangeOfOrdersDetailsAsync(IEnumerable<OrderDetails> entities);
    }
}
