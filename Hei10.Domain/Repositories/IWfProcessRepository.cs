using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Models.Workflow;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Enums;

using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IWfProcessRepository : IRepository<WfProcess>
    {
        Task<WfProcess> GetBySource(TableSource src);
        /// <summary>
        /// 返回所有列表，可以查询
        /// </summary>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        IPagedList<WfProcess> GetPagedList(string keyWord, string orderField, string orderDirection,
            int pageIndex, int pageSize);

        Task<bool> HasPublishedProcess(TableSource source);

        Task<bool> IsExsitName(string name);
    }
}
