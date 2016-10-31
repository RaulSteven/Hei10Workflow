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
    public interface IPartnerRepository : IRepository<Partner>
    {
        IPagedList<Partner> GetList(string name, string orderField, string orderDirection, int pageIndex, int pageSize);

        Task<List<Partner>> GetListAsync();
    }
}
