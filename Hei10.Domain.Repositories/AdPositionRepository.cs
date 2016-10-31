using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Extensions;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using Hei10.Core.Cache;

namespace Hei10.Domain.Repositories
{
    public class AdPositionRepository : Repository<AdPosition>, IAdPositionRepository
    {
        #region Cache

        public ICacheManager Cache { get; set; }

        public string GetByPosKeyCacheKey
        {
            get
            {
                return CacheKey + "GetByPosKeyCache-{0}";
            }
        }

        public AdPosition GetByPosKeyCache(AdPosKey value)
        {
            var result = Cache.Get(string.Format(GetByPosKeyCacheKey, value), () =>
            {
                var model = QueryUnDelete().FirstOrDefault(m => m.Code == value);
                return model ?? new AdPosition();
            });
            return result;
        }

        public override void RemoveCache(AdPosition entity)
        {
            Cache.RemoveByPattern(CacheKey);
        }

        #endregion

        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        public List<ArticleClassifyBizModel> GetListByZTreeAsync()
        {
            var list = AdminQueryUnDelete().OrderBy(m=>m.Sort).Select(d => new ArticleClassifyBizModel()
            {
                id = d.Id,
                name = d.Name,
                pid = (int)d.Code
            }).ToList();
            var pos = AdPosKey.HomeSlide.GetDescriptDict();
            list.AddRange(pos.Select(adPosKey => new ArticleClassifyBizModel
            {
                id = adPosKey.Key, name = adPosKey.Value
            }));
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
            return result;
        }
    }
}
