using DevicesHub.Domain.Interfaces;
using DevicesHub.Domain.Models;
using DevicesHub.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Infrastructure.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(DeviceHubDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}
