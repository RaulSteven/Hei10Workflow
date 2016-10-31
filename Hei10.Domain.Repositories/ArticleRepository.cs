using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        /// <summary>
        /// 通过搜索返回当前分类下所有的记录
        /// </summary>
        /// <param name="keyWord">搜索关键词</param> 
        /// <param name="classifyId">当前分类集合</param> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        public IPagedList<Article> GetPagedList(string keyWord, long classifyId, int pageIndex, int pageSize)
        {
            var query = QueryEnable().Where(d => d.ClassifyId == classifyId);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(d => d.Title.Contains(keyWord));
            }
            return query.OrderByDescending(d => d.ArticleDateTime).ToPagedList(pageIndex, pageSize);
        }

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
        public IPagedList<ArticleBizModel> GetPagedList(IQueryable<ArticleClassify> articleClassifyQuery, string keyWord,long? classifyId, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete().Select(d => new ArticleBizModel { Article = d ,ArticleClassifyName = articleClassifyQuery.Any(m => m.Id == d.ClassifyId) ? articleClassifyQuery.FirstOrDefault(m => m.Id == d.ClassifyId).Name : "" });
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(d => d.Article.Title.Contains(keyWord) ||
                                         d.Article.Author.Contains(keyWord));
            }
            if (classifyId.HasValue)
            {
                query = query.Where(d => d.Article.ClassifyId == classifyId);
            }
            return ToPageList(query, orderField, orderDirection, m => m.Article.ArticleDateTime, pageIndex, pageSize); 
        }

        /// <summary>
        /// 返回当前文章分类所有其下属分类及其文章信息
        /// </summary>
        /// <param name="articleClassifyQuery">文章分类信息</param>
        /// <param name="treePath">当前分类的treePath</param> 
        /// <returns></returns>
        public Task<List<ArticleBizModel>> GetListByTreePathAsync(IQueryable<ArticleClassify> articleClassifyQuery,string treePath)
        { 
            var query = QueryEnable().Select(d => new { Article = d,Classify = articleClassifyQuery.FirstOrDefault(m => m.Id == d.ClassifyId) });
            return query.Where(d => d.Classify != null && d.Classify.TreePath.Contains(treePath))
                        .Select(d => new ArticleBizModel {Article = d.Article,ArticleClassifyName = d.Classify.Name})
                        .OrderByDescending(d => d.Article.ArticleDateTime)
                        .ToListAsync();

        } 

        /// <summary>
        /// 判断该文章分类是否有文章记录
        /// </summary>
        /// <param name="classifyId">当前文章分类的Id</param>
        /// <returns></returns>
        public Task<bool> IsAnyRecordAsync(long classifyId)
        {
            return QueryUnDelete().AnyAsync(d => d.ClassifyId == classifyId);
        }

        /// <summary>
        /// 随机返回当前文章分类所有其下属分类及其文章信息
        /// </summary>
        /// <param name="articleClassifyQuery">文章分类信息</param>
        /// <param name="treePath">当前分类的treePath</param>
        /// <param name="takeCount">返回的数量</param>
        /// <param name="removeId">排除的Id</param>
        /// <returns></returns>
        public Task<List<ArticleBizModel>> GetListByRandomAsyn(IQueryable<ArticleClassify> articleClassifyQuery, string treePath,int takeCount,long removeId)
        {
            var query = QueryEnable().Select(d => new { Article = d, Classify = articleClassifyQuery.FirstOrDefault(m => m.Id == d.ClassifyId),Guid = Guid.NewGuid().ToString() });
            return query.OrderByDescending(d => d.Guid)
                        .Where(d => d.Article != null && d.Classify.TreePath.Contains(treePath) && d.Article.Id != removeId)
                        .Select(d => new ArticleBizModel { Article = d.Article, ArticleClassifyName = d.Classify.Name })
                        .Take(takeCount)
                        .ToListAsync();
        }
    }
}
