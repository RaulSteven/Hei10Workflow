using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework;
using Hei10.Domain.ViewModels;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class SysApartmentRepository : Repository<SysApartment>, ISysApartmentRepository
    {
        public async Task<List<SysApartJsonModel>> GetJsonList()
        {
            var list = await QueryEnable()
                .Select(m => new SysApartJsonModel()
                {
                    id = m.Id,
                    name = m.Name,
                    pid = m.Pid,
                    remark = m.Remark,
                    sort = m.Sort
                })
                .OrderBy(m => m.sort)
                .ThenBy(m => m.id)
                .ToListAsync();
            var result = list.Where(m => m.pid == 0).ToList();
            foreach (var apart in result)
            {
                SetChildren(apart, list);
            }
            return result;
        }

        private void SetChildren(SysApartJsonModel target, List<SysApartJsonModel> all)
        {
            if (all.Any(m => m.pid == target.id))
            {
                target.children = all.Where(m => m.pid == target.id)
                    .OrderBy(m => m.sort)
                    .ThenBy(m => m.id)
                    .ToList();
                foreach (var child in target.children)
                {
                    SetChildren(child, all);
                }
            }
        }

        public override async Task SaveAsync(SysApartment entity)
        {
            await base.SaveAsync(entity);

            entity.TreePath = "";
            if (entity.Pid != 0)
            {
                var parent = await GetByIdAsync(entity.Pid);
                entity.TreePath = parent.TreePath + ".";
            }
            entity.TreePath += entity.Id.ToString(TreePathFormat);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var treePath = id.ToString(TreePathFormat);
            var list = await GetListByTreePathAsync(treePath);
            if (list.Any())
            {
                list.ForEach(m => m.CommonStatus = Enums.CommonStatus.Deleted);
                await SaveChangesAsync();
            }
        }

        private Task<List<SysApartment>> GetListByTreePathAsync(string treePath)
        {
            var list = QueryUnDelete()
                .Where(m => m.TreePath.Contains(treePath))
                .ToListAsync();
            return list;
        }

        /// <summary>
        /// 返回部门所有的记录及部门下所有人员，用于树形结构
        /// </summary>
        /// <param name="userApartmentQuery">部门用户信息</param>
        /// <param name="userQuery">用户信息</param>
        /// <returns></returns>
        public async Task<List<SysApartZTreeModel>> GetListByZTree(IQueryable<User2Apartment> userApartmentQuery,
            IQueryable<Users> userQuery)
        {
            var list = await QueryUnDelete().Select(d => new SysApartZTreeModel()
            {
                id = d.Id,
                pid = d.Pid,
                name = d.Name,
                treePath = d.TreePath,
                userTip = "",
                open = d.Pid == 0,
                sort = d.Sort
            })
                .OrderByDescending(d => d.treePath)
                .ToListAsync();

            var query = QueryUnDelete();
            var user =
                userApartmentQuery.Select(
                    d => new { User2Apartment = d, User = userQuery.FirstOrDefault(m => m.Id == d.UserId) })
                    .Where(d => d.User != null)
                    .Select(
                        d =>
                            new
                            {
                                User = d.User,
                                Apartment = query.FirstOrDefault(m => m.Id == d.User2Apartment.ApartmentId)
                            })
                    .Where(d => d.Apartment != null)
                    .Select(d => new SysApartZTreeModel
                    {
                        id = d.User.Id,
                        pid = d.Apartment.Id,
                        name = d.User.RealName,
                        treePath = d.Apartment.TreePath,
                        userTip = "u_",
                        open = false,
                        sort = d.Apartment.Sort
                    })
                    .OrderByDescending(d => d.treePath)
                    .ToList();
            var result = new List<SysApartZTreeModel>();
            foreach (var item in list)
            {
                item.children = result.Where(d => d.pid == item.id)
                    .OrderBy(d => d.treePath)
                    .ToList();

                if (item.children.Any())
                {
                    result.RemoveAll(d => item.children.Any(m => m.id == d.id));
                }
                var userList = user.Where(d => d.pid == item.id)
                    .OrderBy(d => d.treePath)
                    .ToList();
                if (userList.Any())
                {
                    item.children.AddRange(userList);
                }
                result.Add(item);
            }
            return result.OrderBy(d => d.treePath).ToList();
        }
        
        public async Task<int> GetIndexOfParent(SysApartment apart)
        {
            var list =await QueryEnable()
                .Where(m => m.Pid == apart.Pid)
                .OrderBy(m=>m.Sort)
                .ThenBy(m=>m.Id)
                .ToListAsync();
            var index = list.IndexOf(apart);
            return index;
        }
    }
}
