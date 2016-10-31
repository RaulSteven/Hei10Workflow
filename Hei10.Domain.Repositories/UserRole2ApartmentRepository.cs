using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework;
using Hei10.Core.Utilities;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class UserRole2ApartmentRepository : Repository<UserRole2Apartment>, IUserRole2ApartmentRepository
    {
        public Task<List<long>> GetRoleIdListAsync(long apartId)
        {
            var list = QueryEnable()
                .Where(m => m.ApartmentId == apartId)
                .Select(m => m.RoleId)
                .ToListAsync();
            return list;
        }

        public async Task SaveListAsync(long apartId, string roleIds)
        {
            var delList = await QueryEnable()
                .Where(m => m.ApartmentId == apartId)
                .ToListAsync();
            Table.RemoveRange(delList);

            var idList = StringUtility.ConvertToBigIntArray(roleIds, ',');
            foreach (var roleId in idList)
            {
                var role2Apart = new UserRole2Apartment()
                {
                    ApartmentId = apartId,
                    RoleId = roleId,
                    CommonStatus = Enums.CommonStatus.Enabled
                };
                await SaveAsync(role2Apart);
            }
        }
    }
}
