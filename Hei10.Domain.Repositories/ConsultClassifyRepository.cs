using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class ConsultClassifyRepository : Repository<ConsultClassify>, IConsultClassifyRepository
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
        public IPagedList<ConsultClassify> GetPagedList(string keyWord, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete();
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(d => d.Title.Contains(keyWord));
            } 
            return ToPageList(query, orderField, orderDirection, m => m.CreateTime, pageIndex, pageSize);
        }

        /// <summary>
        /// 返回所有的咨询分类,用于下拉框
        /// </summary>
        /// <returns></returns>
        public Task<List<SelectListItem>> GetListBySelectAsync()
        {
            return AdminQueryUnDelete().Select(d => new SelectListItem {Value = d.Id.ToString(), Text = d.Title})
                                       .ToListAsync();
        }
    }
}
