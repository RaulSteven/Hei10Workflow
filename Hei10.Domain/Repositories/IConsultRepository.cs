using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IConsultRepository : IRepository<Consult>
    {
        /// <summary>
        /// 返回所有的咨询记录
        /// </summary> 
        /// <param name="consultClassifyQuery">咨询分类信息</param>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="classifyId">咨询分类</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        IPagedList<ConsultBizModel> GetPagedList(IQueryable<ConsultClassify> consultClassifyQuery, string keyWord, long? classifyId, string orderField, string orderDirection, int pageIndex, int pageSize);
    }
}
