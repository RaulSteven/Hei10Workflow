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
using Hei10.Domain.Enums;

namespace Hei10.Domain.Repositories
{
    public class UserRole2MenuRepository : Repository<UserRole2Menu>, IUserRole2MenuRepository
    {
        public async Task SaveListAsync(long roleId, string menuIds)
        {
            var idList = StringUtility.ConvertToBigIntArray(menuIds, ',');
            var oldList = await QueryEnable()
                .Where(m => m.RoleId == roleId)
                .ToListAsync();
            //删除不存在的
            var oldDeleList = oldList.Where(m => !idList.Contains(m.MenuId))
                .ToList();
            Table.RemoveRange(oldDeleList);
            await SaveChangesAsync();

            //依然保持的数据
            var oldHoldList = oldList.Where(m => idList.Contains(m.MenuId))
                .Select(m => m.MenuId)
                .ToList();

            //新增的数据
            var newIdList = idList.Except(oldHoldList);
            foreach (var menuId in newIdList)
            {
                var role2menu = new UserRole2Menu()
                {
                    RoleId = roleId,
                    MenuId = menuId,
                    CommonStatus = Enums.CommonStatus.Enabled
                };
                await SaveAsync(role2menu);
            }
        }

        public Task<List<long>> GetMenuIdListAsync(long roleId)
        {
            var list = QueryEnable()
                .Where(m => m.RoleId == roleId)
                .Select(m => m.MenuId)
                .ToListAsync();
            return list;
        }

        public IQueryable<UserRole2Menu> GetQuery(long roleId)
        {
            var list = QueryEnable()
                .Where(m => m.RoleId == roleId);
            return list;
        }

        public async Task<bool> UpdateFilterGroups(long id, string filterGroups)
        {
            var obj = await GetEnableByIdAsync(id);
            if (obj == null)
            {
                return false;
            }
            obj.FilterGroups = filterGroups;
            await SaveAsync(obj);
            return true;
        }

        public async Task SaveRole2MenuButtons(long roleId, string btnIds)
        {
            #region check params

            if (string.IsNullOrEmpty(btnIds))
            {
                return;
            }
            var idArr = btnIds.Split(',');
            if (idArr == null || idArr.Length == 0)
            {
                return;
            }
            if (roleId == 0)
            {
                return;
            }

            #endregion
            var menuAllList = await QueryEnable()
                .Where(m => m.RoleId == roleId)
                .ToListAsync();
            
            var role2MenuList = new List<UserRole2Menu>();
            foreach (var r2mId in idArr)
            {
                var r2mArr = StringUtility.ConvertToBigIntArray(r2mId, '_');
                var role2Menu = role2MenuList.FirstOrDefault(m => m.Id == r2mArr[0]);
                if (role2Menu == null)
                {
                    role2Menu = menuAllList.FirstOrDefault(m=>m.Id == r2mArr[0]);
                    role2Menu.Buttons = SysButton.None;
                    role2MenuList.Add(role2Menu);
                }
                role2Menu.Buttons = role2Menu.Buttons | (SysButton)r2mArr[1];
            }
            var noneButtonMenuList = menuAllList
                .Where(m => !role2MenuList.Any(r => r.Id == m.Id))
                .ToList();
            noneButtonMenuList.ForEach(m => m.Buttons = SysButton.None);

            await SaveChangesAsync();
        }
    }
}
