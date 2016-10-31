using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Repositories;
using Hei10.Domain.Models;
using Hei10.Core.Cache;
using Hei10.Core.Utilities;
using Hei10.Domain.ViewModels;
using System.Data.Entity;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Enums;
using Hei10.Core.Extensions;

namespace Hei10.Domain.Services
{
    public class UserRoleSvc : IUserRoleSvc
    {
        public IUser2RoleRepository User2RoleRepository { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }
        public IUserRole2ApartmentRepository Role2ApartRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public ICacheManager Cache { get; set; }
        public IUserRole2MenuRepository Role2MenuRepository { get; set; }
        public ISysMenuRepository SysMenuRepository { get; set; }
        public IUsersRepository UserRepository { get; set; }
        public ISysApartmentRepository SysApartRepository { get; set; }
        public IUserRole2FilterRepository Role2FilterRepository { get; set; }

        public List<long> GetRoleIdList(long userId)
        {
            if (userId == 0)
            {
                return new List<long>();
            }
            //用户x角色
            var user2RoleIdList = User2RoleRepository
                .QueryEnable()
                .Where(m => m.UserId == userId)
                .Select(m => m.RoleId)
                .ToList();

            //用户x部门x角色
            var user2ApartQuery = User2ApartRepository
                .QueryEnable()
                .Where(m => m.UserId == userId);
            var role2ApartQuery = Role2ApartRepository
                .QueryEnable();
            var apart2RoleIdList = user2ApartQuery
                .Join(role2ApartQuery,
                    u2a => u2a.ApartmentId,
                    r2a => r2a.ApartmentId,
                    (u2a, r2a) => r2a.RoleId);

            //RoleIdList
            user2RoleIdList.AddRange(apart2RoleIdList);
            var roleIdList = user2RoleIdList
                .Distinct()
                .ToList() ;
            return roleIdList;
        }

        public async Task SaveListAsync(long roleId, string menuIds)
        {
            await Role2MenuRepository.SaveListAsync(roleId, menuIds);

            ClearRoleUserCache(roleId);
        }

        public void ClearRoleUserCache(long roleId)
        {
            //user2role 找到userid
            var userIdLst = User2RoleRepository
                .QueryEnable()
                .Where(m => m.RoleId == roleId)
                .Select(m => m.UserId)
                .ToList();

            //user2apart  role2apart 找到userid
            var user2ApartQuery = User2ApartRepository
                .QueryEnable();
            var role2ApartQuery = Role2ApartRepository
                .QueryEnable()
                .Where(m => m.RoleId == roleId);
            var apart2UserIdList = user2ApartQuery
                .Join(role2ApartQuery,
                    u2a => u2a.ApartmentId,
                    r2a => r2a.ApartmentId,
                    (u2a, r2a) => u2a.UserId);

            //合集
            userIdLst.AddRange(apart2UserIdList);

            //剔除重复
            userIdLst = userIdLst.Distinct().ToList();

            //清除缓存
            foreach (var userId in userIdLst)
            {
                Cache.Remove(Users.GIdPrefix + userId);
            }
        }

        public Task<List<DropdownItemModel>> GetResListAsync(FilterCurrent curr)
        {
            IQueryable<DropdownItemModel> query = null;
            switch (curr)
            {
                case FilterCurrent.CurrentUserId:
                    query = UserRepository.QueryEnable()
                        .OrderByDescending(m => m.Id)
                        .Select(m => new DropdownItemModel()
                        {
                            Value = m.Id,
                            Text = m.LoginName
                        });
                    break;
                case FilterCurrent.CurrentRoleId:
                    query = UserRoleRepository.QueryEnable()
                        .OrderBy(m => m.Sort)
                        .ThenByDescending(m => m.Id)
                        .Select(m => new DropdownItemModel()
                        {
                            Value = m.Id,
                            Text = m.Name
                        });
                    break;
                case FilterCurrent.CurrentDeptId:
                    query = SysApartRepository.QueryEnable()
                        .OrderBy(m => m.Sort)
                        .ThenByDescending(m => m.Id)
                        .Select(m => new DropdownItemModel()
                        {
                            Value = m.Id,
                            Text = m.Name
                        });
                    break;
                default:
                    break;
            }
            return query.ToListAsync();
        }
        public List<FilterGroupModel> GetFilterList(long roleId)
        {
            var list = Role2FilterRepository
                .QueryEnable()
                .Where(m => m.RoleId == roleId)
                .Select(m=> new FilterGroupModel()
                {
                    Id = m.Id,
                    Source = m.Source,
                    FilterGroups = m.FilterGroups,
                    Name = m.Name
                })
                .ToList();
            foreach (var item in list)
            {
                #region 加载属性
                var type = Assembly.Load("Hei10.Domain").GetType("Hei10.Domain.Models." + item.Source);
                var propModelList = new List<PropertyModel>();
                var propertyInfoList = type.GetProperties();
                foreach (var property in propertyInfoList)
                {
                    var proModel = new PropertyModel()
                    {
                        Name = property.Name,
                        TypeName = property.PropertyType.IsEnum ? "Enum" : property.PropertyType.Name,
                    };
                    var nameAttr = property.GetCustomAttribute(typeof(DisplayNameAttribute));
                    if (nameAttr == null)
                    {
                        proModel.DisplayName = property.Name;
                    }
                    else
                    {
                        proModel.DisplayName = ((DisplayNameAttribute)nameAttr).DisplayName;
                    }
                    proModel.DisplayName = $"{proModel.DisplayName}({proModel.TypeName})";
                    propModelList.Add(proModel);
                }
                foreach (var curr in FilterCurrent.CurrentDeptId.GetSList())
                {
                    propModelList.Add(new PropertyModel()
                    {
                        Name = curr.Value,
                        DisplayName = curr.Text,
                        TypeName = "Enum"
                    });
                }
                item.SourceProperties = JsonConvert.SerializeObject(propModelList);
                #endregion

            }
            return list;
        }

        public Task<List<Users>> GetUserListByRoleIds(string roleIds)
        {
            if (string.IsNullOrEmpty(roleIds))
            {
                return Task.FromResult<List<Users>>(null);
            }
            var roleIdArray = StringUtility.ConvertToBigIntArray(roleIds, ',');

            var userIdList = User2RoleRepository
                .AdminQueryEnable()
                .Where(m => roleIdArray.Contains(m.RoleId))
                .Select(m=>m.UserId)
                .Distinct();
            var userList = UserRepository
                .AdminQueryEnable()
                .Where(m => userIdList.Contains(m.Id))
                .ToListAsync();
            return userList;
        }
    }
}
