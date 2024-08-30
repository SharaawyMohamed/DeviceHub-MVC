using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;

namespace DevicesHub.Domain.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
	{
	}
}
