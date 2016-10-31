using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Repositories
{
    public interface ISysMenuRepository:IRepository<SysMenu>
    {
        Task<List<SysMenuJsonModel>> GetJsonList();

        Task DeleteAsync(long id);
        Task<int> GetIndexOfParent(SysMenu menu);

    }
}
