using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class PartnerRepository : Repository<Partner>, IPartnerRepository
    {
        public IPagedList<Partner> GetList(string name,
            string orderField,
            string orderDirection, 
            int pageIndex,
            int pageSize)
        {
            var query = AdminQueryEnable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }
            var list = ToPageList(query, orderField, orderDirection, m => m.Sort, pageIndex, pageSize);
            return list;
        }

        public Task<List<Partner>> GetListAsync()
        {
            var list = QueryEnable()
                .OrderBy(m => m.Sort)
                .ThenBy(m => m.Id)
                .ToListAsync();
            return list;
        }
    }
}
