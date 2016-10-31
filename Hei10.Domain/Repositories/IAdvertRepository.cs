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
    public interface IAdvertRepository : IRepository<Advert>
    {
        IPagedList<AdvertBizModel> GetList(long adPosId, string name, AdvertStatus? status, string orderField, string orderDirection,
            int pageIndex, int pageSize);
        List<Advert> GetListByAdPostIdCache(long id, AdvertType? type, int takeSize);
    }
}
