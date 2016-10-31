using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class RecruitJobRepository : Repository<RecruitJob>, IRecruitJobRepository
    {
        public IPagedList<RecruitJob> GetPagedList(int pageCurrent, int pageSize, string orderField, string orderDirection, string name)
        {
            var query = AdminQueryUnDelete();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(l => l.Name.Contains(name));
            }
            return ToPageList(query, orderField, orderDirection,m=>m.Sort, pageCurrent, pageSize);
        }

        public Task<List<RecruitJob>> GetListAsync()
        {
            var list = AdminQueryEnable().OrderBy(m => m.Sort).ThenByDescending(m => m.CreateTime).ToListAsync();
            return list;
        } 
    }
}
