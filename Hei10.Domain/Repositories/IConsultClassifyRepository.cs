using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IConsultClassifyRepository : IRepository<ConsultClassify>
    {
        /// <summary>
        /// 返回所有的咨询分类记录
        /// </summary> 
        /// <param name="keyWord">搜索关键词</param> 
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        IPagedList<ConsultClassify> GetPagedList(string keyWord, string orderField, string orderDirection, int pageIndex,
            int pageSize);

        /// <summary>
        /// 返回所有的咨询分类,用于下拉框
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetListBySelectAsync();
    }
}
