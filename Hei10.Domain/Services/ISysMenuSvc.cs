using Hei10.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Services
{
    public interface ISysMenuSvc
    {
        Task<List<SysMenuJsonModel>> GetJsonListAsync(long roleId);

        Task<List<SysUserRole2MenuModel>> GetRole2MenuListAsync(long roleId);

        List<UserMenuModel> GetList(List<long> lstRoleId);
    }
}
