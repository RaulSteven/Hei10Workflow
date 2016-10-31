using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface IUser2RoleRepository : IRepository<User2Role>
    {
        Task<List<long>> GetRoleIdListAsync(long userId);
        Task SaveListAsync(long userId, string roleIds);
    }

}
