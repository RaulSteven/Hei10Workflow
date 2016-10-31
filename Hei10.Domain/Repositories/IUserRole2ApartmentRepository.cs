using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface IUserRole2ApartmentRepository : IRepository<UserRole2Apartment>
    {
        Task<List<long>> GetRoleIdListAsync(long apartId);
        Task SaveListAsync(long apartId, string roleIds);
    }
}
