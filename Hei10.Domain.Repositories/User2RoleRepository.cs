using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework;
using System.Data.Entity;
using Hei10.Core.Utilities;
using Hei10.Core.Cache;

namespace Hei10.Domain.Repositories
{
    public class User2RoleRepository : Repository<User2Role>, IUser2RoleRepository
    {
        public ICacheManager Cache { get; set; }
        public Task<List<long>> GetRoleIdListAsync(long userId)
        {
            var list = QueryEnable()
                .Where(m => m.UserId == userId)
                .Select(m => m.RoleId)
                .ToListAsync();
            return list;
        }

        public async Task SaveListAsync(long userId, string roleIds)
        {
            var delList = await QueryEnable()
                .Where(m => m.UserId == userId)
                .ToListAsync();
            Table.RemoveRange(delList);

            var idList = StringUtility.ConvertToBigIntArray(roleIds, ',');
            foreach (var roleId in idList)
            {
                var role2menu = new User2Role()
                {
                    UserId = userId,
                    RoleId = roleId,
                    CommonStatus = Enums.CommonStatus.Enabled
                };
                await SaveAsync(role2menu);
            }
            Cache.Remove(Users.GIdPrefix + userId);
        }
    }
}
