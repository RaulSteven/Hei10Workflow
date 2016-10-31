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
    public interface IFrontMenuRepository : IRepository<FrontMenu>
    {
        Task<List<FrontMenuJsonModel>> GetJsonListAsync();

        Task DeleteAsync(long id);

        List<FrontMenuJsonModel> GetJsonListCache();
        Task<int> GetIndexOfParent(FrontMenu menu);
    }
}
