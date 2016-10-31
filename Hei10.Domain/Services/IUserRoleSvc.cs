using Hei10.Domain.Enums;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hei10.Domain.Services
{
    public interface IUserRoleSvc
    {
        /// <summary>
        /// 获取该用户的角色Id = 用户x角色 + 用户x部门x角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<long> GetRoleIdList(long userId);

        Task SaveListAsync(long roleId, string menuIds);

        void ClearRoleUserCache(long roleId);
        
        Task<List<DropdownItemModel>> GetResListAsync(FilterCurrent curr);

        List<FilterGroupModel> GetFilterList(long roleId);

        Task<List<Users>> GetUserListByRoleIds(string roleIds);
    }
}
