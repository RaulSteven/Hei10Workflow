using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Repositories
{
    public interface IAdPositionRepository : IRepository<AdPosition>
    {
        /// <summary>
        /// 返回下拉列表
        /// </summary>
        /// <returns></returns>
        List<ArticleClassifyBizModel> GetListByZTreeAsync();
        AdPosition GetByPosKeyCache(AdPosKey value);
    }
}
