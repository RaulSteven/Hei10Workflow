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
    public interface IArticleClassifyRepository : IRepository<ArticleClassify>
    {
        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleClassifyBizModel>> GetListByZTreeAsync();

        /// <summary>
        /// 添加或修改数据
        /// </summary>  
        /// <param name="model">当前分类</param>
        /// <param name="p">当前主分类</param> 
        Task SaveAsync(ArticleClassify model, ArticleClassify p); 

        /// <summary>
        /// 查看是否有子分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        Task<bool> IsLastChildAsync(long id);
         
    }
}
