using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework;
using PagedList;
using System.Data.SqlClient;
using Hei10.Domain.Enums;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public async Task Delete(long id)
        {
            //删除角色
            await Factory.Context.Database
                 .ExecuteSqlCommandAsync("Update UserRoles set CommonStatus = @delStatus where Id = @id",
                  new SqlParameter("@delStatus", CommonStatus.Deleted),
                  new SqlParameter("@id", id));

            //删除关联用户信息
            await Factory.Context.Database
                 .ExecuteSqlCommandAsync("Update User2Role set CommonStatus = @delStatus where RoleId = @id",
                  new SqlParameter("@delStatus", CommonStatus.Deleted),
                  new SqlParameter("@id", id));

            //删除关联菜单信息
            await Factory.Context.Database
                 .ExecuteSqlCommandAsync("Update UserRole2Menu set CommonStatus = @delStatus where RoleId = @id",
                  new SqlParameter("@delStatus", CommonStatus.Deleted),
                  new SqlParameter("@id", id));

            //删除关联部门信息
            await Factory.Context.Database
                 .ExecuteSqlCommandAsync("Update UserRole2Apartment set CommonStatus = @delStatus where RoleId = @id",
                  new SqlParameter("@delStatus", CommonStatus.Deleted),
                  new SqlParameter("@id", id));
        }

        public IPagedList<UserRole> GetList(int pageSize, int pageCurrent)
        {
            var roleList = AdminQueryEnable()
                .OrderBy(m => m.Sort)
                .ToPagedList(pageCurrent,pageSize );
            return roleList;
        }

        public Task<List<UserRole>> GetListAsync()
        {
            var list = QueryEnable()
                .OrderBy(m => m.Sort)
                .ToListAsync();
            return list;
        }
    }
}
