using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface IUserRole2FilterRepository : IRepository<UserRole2Filter>
    {
        List<UserRole2Filter> GetRoleFilterList(List<long> roleId);

        /// <summary>
        /// 是否存在数据规则
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="source">资源</param>
        /// <returns></returns>
        Task<bool> ExistSourceAsync(long id, string source);
    }
}
