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
    public class SysMenuRepository : Repository<SysMenu>, ISysMenuRepository
    {
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

        private Task<List<SysMenu>> GetListByTreePathAsync(string treePath)
        {
            var list = QueryUnDelete()
                .Where(m => m.TreePath.Contains(treePath))
                .ToListAsync();
            return list;
        }

        public async Task<List<SysMenuJsonModel>> GetJsonList()
        {
            var list = await QueryEnable()
                .Select(m => new SysMenuJsonModel()
                {
                    id = m.Id,
                    name = m.Name,
                    pid = m.Pid,
                    remark = m.Remark,
                    sort = m.Sort,
                    url = m.Url,
                    faicon = m.Icon,
                    source = m.Source,
                    buttons = m.Buttons
                })
                .OrderBy(m => m.sort)
                .ThenBy(m => m.id)
                .ToListAsync();
            var result = list.Where(m => m.pid == 0).ToList();
            foreach (var item in result)
            {
                SetChildren(item, list);
            }
            return result;
        }

        private void SetChildren(SysMenuJsonModel target, List<SysMenuJsonModel> all)
        {
            if (all.Any(m => m.pid == target.id))
            {
                target.children = all.Where(m => m.pid == target.id).ToList();
                foreach (var child in target.children)
                {
                    SetChildren(child, all);
                }
            }
        }

        public override async Task SaveAsync(SysMenu entity)
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
        public async Task<int> GetIndexOfParent(SysMenu menu)
        {
            var list = await QueryEnable()
                .Where(m => m.Pid == menu.Pid)
                .OrderBy(m => m.Sort)
                .ThenBy(m => m.Id)
                .ToListAsync();
            var index = list.IndexOf(menu);
            return index;
        }
    }
}
