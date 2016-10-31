using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;
using System.Data.Entity;
using PagedList;


namespace Hei10.Domain.Repositories
{
    public class WfProcessRepository : Repository<WfProcess>, IWfProcessRepository
    {
        public Task<WfProcess> GetBySource(TableSource src)
        {
            var obj = AdminQueryEnable()
                .FirstOrDefaultAsync(m => m.TableSource == src);
            return obj;
        }
        /// <summary>
        /// 返回所有列表，可以查询
        /// </summary>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        public IPagedList<WfProcess> GetPagedList(string keyWord, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete();
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(m => m.Name.Contains(keyWord));
            }

            return ToPageList(query, orderField, orderDirection, m => m.UpdateTime, pageIndex, pageSize);
        }

        public async Task<bool> HasPublishedProcess(TableSource source)
        {
            return await AdminQueryEnable()
                .AnyAsync(m => m.TableSource == source && m.CommonStatus == CommonStatus.Enabled);
        }

        public async Task<bool> IsExsitName(string name)
        {
            return await AdminQueryEnable()
                .AnyAsync(m => m.Name == name);
        }
    }
}
