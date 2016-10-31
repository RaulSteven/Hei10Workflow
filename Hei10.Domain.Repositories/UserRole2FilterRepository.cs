using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class UserRole2FilterRepository : Repository<UserRole2Filter>, IUserRole2FilterRepository
    {
        public List<UserRole2Filter> GetRoleFilterList(List<long> roleId)
        {
            var list = QueryEnable()
                .Where(m => roleId.Contains(m.RoleId))
                .ToList();
            return list;
        }

        /// <summary>
        /// 是否存在数据规则
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="source">资源</param>
        /// <returns></returns>
        public Task<bool> ExistSourceAsync(long id, string source)
        {
            return QueryUnDelete().AnyAsync(d => d.Id != id && d.Source == source);
        }
    }
}
