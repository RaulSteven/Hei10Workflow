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
    public interface IRecruitJobRepository : IRepository<RecruitJob>
    {
        IPagedList<RecruitJob> GetPagedList(int pageCurrent, int pageSize, string orderField, string orderDirection, string name);

        Task<List<RecruitJob>> GetListAsync();
    }
}
