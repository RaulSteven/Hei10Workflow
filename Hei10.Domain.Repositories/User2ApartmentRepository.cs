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
    public class User2ApartmentRepository : Repository<User2Apartment>, IUser2ApartmentRepository
    {
        public ICacheManager Cache { get; set; }
        public Task<List<long>> GetApartIdListAsync(long userId)
        {
            var list = QueryEnable()
                .Where(m => m.UserId == userId)
                .Select(m => m.ApartmentId)
                .ToListAsync();
            return list;
        }

        public async Task SaveListAsync(long id, string apartIds)
        {
            var delList = await QueryEnable()
               .Where(m => m.UserId == id)
               .ToListAsync();
            Table.RemoveRange(delList);

            var idList = StringUtility.ConvertToBigIntArray(apartIds, ',');
            foreach (var apartId in idList)
            {
                var user2Apart = new User2Apartment()
                {
                    UserId  = id,
                    ApartmentId = apartId,
                    CommonStatus = Enums.CommonStatus.Enabled
                };
                await SaveAsync(user2Apart);
            }
            Cache.Remove(Users.GIdPrefix + id);

        }
    }
}
