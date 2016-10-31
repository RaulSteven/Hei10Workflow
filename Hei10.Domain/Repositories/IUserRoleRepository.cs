using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        IPagedList<UserRole> GetList(int pageSize, int pageCurrent);

        Task Delete(long id);

        Task<List<UserRole>> GetListAsync();
    }
}
