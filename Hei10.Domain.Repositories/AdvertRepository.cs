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
using Hei10.Core.Cache;

namespace Hei10.Domain.Repositories
{
    public class AdvertRepository : Repository<Advert>, IAdvertRepository
    {
        public IPagedList<AdvertBizModel> GetList(long adPosId, string name, AdvertStatus? status, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete().Where(d => d.AdPosId == adPosId);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }
            if (status.HasValue)
            {
                query = query.Where(d => d.AdvertStatus == status.Value);
            }
            var result = query.Select(d => new AdvertBizModel
            {
                Id = d.Id,
                Name = d.Name,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                Status = d.AdvertStatus,
                AdType = d.AdType,
                CreateDateTime = d.CreateTime,
                SortIndex = d.SortIndex,
                ClassifyName = ""
            });
            return ToPageList(result, orderField, orderDirection, m => m.CreateDateTime, pageIndex, pageSize);
        }

        #region Cache
        public ICacheManager Cache { get; set; }
        private string GetListByAdPostIdKey
        {
            get
            {
                return CacheKey + "-GetListByAdPostIdCache-{0}-{1}-{2}";
            }
        }

        public List<Advert> GetListByAdPostIdCache(long adPosId, AdvertType? type, int takeSize)
        {
            return Cache.Get(string.Format(GetListByAdPostIdKey, adPosId, type, takeSize), () =>
            {
                var query = QueryUnDelete().Where(m => m.AdPosId == adPosId &&
                                             m.AdvertStatus == AdvertStatus.Publish &&
                                             (m.EndTime == null || m.EndTime >= DateTime.Now));
                if (type.HasValue)
                {
                    query = query.Where(m => m.AdType == type.Value);
                }
                if (takeSize > 0)
                {
                    return query.OrderBy(m => m.SortIndex)
                                 .ThenByDescending(m => m.CreateTime)
                                 .Take(takeSize)
                                 .ToList();
                }
                return query.OrderBy(m => m.SortIndex)
                            .ThenByDescending(m => m.CreateTime)
                            .ToList();
            });
        }

        public override void RemoveCache(Advert entity)
        {
            Cache.RemoveByPattern(CacheKey);
        }

        #endregion
    }
}
