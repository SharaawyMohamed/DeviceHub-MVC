using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevicesHub.Infrastructure.Repositories
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly DeviceHubDbContext dbContext;

        public OrderHeaderRepository(DeviceHubDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task UpdateOrderStatus(int Id, string OrderStatus, string PaymentStatus)
        {
            var order =await dbContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == Id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus;
                order.PaymentDate = DateTime.Now;
                if (PaymentStatus != null)
                    order.PaymentStatus = PaymentStatus;
            }
        }
    }
}
