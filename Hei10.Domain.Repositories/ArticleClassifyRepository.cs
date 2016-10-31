using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Repositories
{
    public class ArticleClassifyRepository : Repository<ArticleClassify>, IArticleClassifyRepository
    {
        /// <summary>
        /// 树显示方式
        /// </summary>
        public new const string TreePathFormat = "00000000";
        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleClassifyBizModel>> GetListByZTreeAsync()
        { 
            var list = await AdminQueryUnDelete().Select(d => new ArticleClassifyBizModel()
                                                {
                                                    id = d.Id,
                                                    pid = d.PId,
                                                    name = d.Name, 
                                                    treePath = d.TreePath,
                                                    partialViewCode = d.PartialViewCode+"",
                                                    remark = d.Remark
                                                })
                                                .OrderByDescending(d => d.treePath)
                                                .ToListAsync();

            var result = new List<ArticleClassifyBizModel>();  
            foreach (var item in list)
            {
                item.children = result.Where(d => d.pid == item.id) 
                                      .OrderBy(d => d.treePath)
                                      .ToList();
                if (item.children.Any())
                { 
                    result.RemoveAll(d => item.children.Any(m => m.id == d.id));
                }
                result.Add(item);
            }
            return result.OrderBy(d => d.treePath).ToList();
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>  
        /// <param name="model">当前分类</param>
        /// <param name="p">当前主分类</param> 
        public async Task SaveAsync(ArticleClassify model, ArticleClassify p)
        {
            var oldTreePath = model.TreePath.Trim('-');
            await SaveAsync(model);

            model.TreePath = "";
            if (p != null)
            {
                model.TreePath = p.TreePath + ",";
                p.ChildrenCount = QueryUnDelete().Count(d => d.PId == p.Id);
            }
            model.TreePath += model.Id.ToString(TreePathFormat);
            model.Depth = model.TreePath.Split(',').Count(d => !string.IsNullOrEmpty(d)) - 1;

            if (!string.IsNullOrEmpty(oldTreePath) && model.OldPId != model.PId)//修改主分类的情况修改其子分类的TreePath和Depth，旧的主分类ChildrenCount
            {
                QueryUnDelete().Where(d => (d.TreePath.StartsWith(oldTreePath) && d.Id != model.Id) || d.Id == model.OldPId)
                               .OrderBy(d => d.TreePath)
                               .ToList()
                               .ForEach(item =>
                               {
                                   if (item.Id == model.OldPId)
                                   {
                                       item.ChildrenCount = QueryUnDelete().Count(d => d.PId == model.OldPId);
                                       return;
                                   }
                                   item.TreePath = model.TreePath + item.TreePath.Substring(oldTreePath.Length);
                                   item.Depth = item.TreePath.Split(',').Count(d => !string.IsNullOrEmpty(d)) - 1;
                               });
            }
            await SaveAsync(model);
        } 

        /// <summary>
        /// 查看是否有子分类
        /// </summary>
        /// <param name="id">当前记录Id</param>
        /// <returns></returns>
        public Task<bool> IsLastChildAsync(long id)
        { 
            return QueryUnDelete().AnyAsync(d => d.PId == id && id > 0);
        } 
    }
}
