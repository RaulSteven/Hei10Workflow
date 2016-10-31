using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface IUserRole2MenuRepository:IRepository<UserRole2Menu>
    {
        Task SaveListAsync(long roleId, string menuIds);

        Task SaveRole2MenuButtons(long roleId, string btnIds);

        Task<List<long>> GetMenuIdListAsync(long roleId);

        IQueryable<UserRole2Menu> GetQuery(long roleId);

        Task<bool> UpdateFilterGroups(long id, string filterGroups);
    }
}
