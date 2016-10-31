using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Repositories;
using Hei10.Domain.Models;
using System.Data.Entity;
using Hei10.Domain.Enums;
using Newtonsoft.Json;

namespace Hei10.Domain.Services
{
    public class SysMenuSvc : ISysMenuSvc
    {
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IUserRole2MenuRepository UserRole2MenuReopsitory { get; set; }

        public async Task<List<SysMenuJsonModel>> GetJsonListAsync(long roleId)
        {
            var menuList = await SysMenuRepository.GetJsonList();

            var menuIdList = await UserRole2MenuReopsitory.GetMenuIdListAsync(roleId);

            SetJsonChecked(menuIdList, menuList);

            return menuList;
        }

        private void SetJsonChecked(List<long> menuIdList, List<SysMenuJsonModel> menuList)
        {
            if (menuList == null
                || menuList.Count == 0
                || menuIdList == null
                || menuIdList.Count == 0)
            {
                return;
            }
            foreach (var menu in menuList)
            {
                menu.@checked = menuIdList.Any(m => m == menu.id).ToString().ToLower();
                menu.url = "";
                SetJsonChecked(menuIdList, menu.children);
            }
        }

        public Task<List<SysUserRole2MenuModel>> GetRole2MenuListAsync(long roleId)
        {
            var queryRole2Menu = UserRole2MenuReopsitory.GetQuery(roleId);
            var list = SysMenuRepository.QueryEnable()
                .Where(m => m.Buttons != SysButton.None)
                .Join(queryRole2Menu,
                    menu => menu.Id,
                    r2m => r2m.MenuId,
                    (menu, r2m) => new SysUserRole2MenuModel()
                    {
                        Id = r2m.Id,
                        Buttons = menu.Buttons,
                        MenuId = menu.Id,
                        MenuName = menu.Name,
                        RoleId = r2m.RoleId,
                        SelectedButtons = r2m.Buttons,
                        MenuTreePath = menu.TreePath
                    })
                .OrderBy(m => m.MenuTreePath)
                .ToListAsync();
            return list;
        }

        public List<UserMenuModel> GetList(List<long> lstRoleId)
        {
            if (lstRoleId == null || !lstRoleId.Any())
            {
                return new List<UserMenuModel>();
            }
            var role2MenuQuery = UserRole2MenuReopsitory
                .QueryEnable()
                .Where(m => lstRoleId.Contains(m.RoleId));
            var menuList = SysMenuRepository.QueryEnable()
                .Join(role2MenuQuery,
                    menu=>menu.Id,
                    r2m=>r2m.MenuId,
                    (menu,r2m)=>menu)
                .ToList();
            var r2mList = role2MenuQuery.ToList();
            List<UserMenuModel> list = new List<UserMenuModel>();
            foreach (var role2menu in r2mList)
            {
                var menuModel = list.FirstOrDefault(m => m.Id == role2menu.MenuId);
                FilterGroup filterGroup = null;
                if (!string.IsNullOrEmpty(role2menu.FilterGroups))
                {
                    filterGroup = JsonConvert.DeserializeObject<FilterGroup>(role2menu.FilterGroups);
                }
                if (menuModel == null)
                {
                    var menu = menuList.FirstOrDefault(m => m.Id == role2menu.MenuId);
                    if (menu == null)
                    {
                        continue;
                    }
                    menuModel = new UserMenuModel()
                    {
                        Id = menu.Id,
                        Icon = menu.Icon,
                        Name = menu.Name,
                        Pid = menu.Pid,
                        Sort = menu.Sort,
                        Source = menu.Source,
                        Url = menu.Url,
                        Buttons = role2menu.Buttons,
                        FilterGroup = filterGroup
                    };
                    list.Add(menuModel);
                    continue;
                }
                menuModel.Buttons = menuModel.Buttons | role2menu.Buttons;
                if (menuModel.FilterGroup == null)
                {
                    menuModel.FilterGroup = filterGroup;
                }
                else if (filterGroup != null)
                {
                    menuModel.FilterGroup = new FilterGroup()
                    {
                        Op = FilterGroupOp.Or,
                        ListGroup = new List<FilterGroup>()
                        {
                            menuModel.FilterGroup,
                            filterGroup
                        }
                    };
                }
            }
            return list;
        }
    }
}
