using DevicesHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Domain.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        Task UpdateOrderStatus(int Id, string OrderStatus, string PaymentStatus);
    }
}
