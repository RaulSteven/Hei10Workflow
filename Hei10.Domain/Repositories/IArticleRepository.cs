using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        /// <summary>
        /// 通过搜索返回当前分类下所有的记录
        /// </summary>
        /// <param name="keyWord">搜索关键词</param> 
        /// <param name="classifyId">当前分类集合</param> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        IPagedList<Article> GetPagedList(string keyWord, long classifyId, int pageIndex, int pageSize);

        /// <summary>
        /// 返回当前分类下所有的文章
        /// </summary>
        /// <param name="articleClassifyQuery">分类信息</param>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="classifyId">当前分类的Id</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        IPagedList<ArticleBizModel> GetPagedList(IQueryable<ArticleClassify> articleClassifyQuery, string keyWord, long? classifyId, string orderField, string orderDirection, int pageIndex, int pageSize);

        /// <summary>
        /// 返回当前文章分类所有其下属分类及其文章信息
        /// </summary>
        /// <param name="articleClassifyQuery">文章分类信息</param>
        /// <param name="treePath">当前分类的treePath</param> 
        /// <returns></returns>
        Task<List<ArticleBizModel>> GetListByTreePathAsync(IQueryable<ArticleClassify> articleClassifyQuery, string treePath);  

        /// <summary>
        /// 判断该文章分类是否有文章记录
        /// </summary>
        /// <param name="classifyId">当前文章分类的Id</param>
        /// <returns></returns>
        Task<bool> IsAnyRecordAsync(long classifyId);

        /// <summary>
        /// 随机返回当前文章分类所有其下属分类及其文章信息
        /// </summary>
        /// <param name="articleClassifyQuery">文章分类信息</param>
        /// <param name="treePath">当前分类的treePath</param>
        /// <param name="takeCount">返回的数量</param>
        /// <param name="removeId">排除的Id</param>
        /// <returns></returns>
        Task<List<ArticleBizModel>> GetListByRandomAsyn(IQueryable<ArticleClassify> articleClassifyQuery, string treePath, int takeCount, long removeId);
    }
}
