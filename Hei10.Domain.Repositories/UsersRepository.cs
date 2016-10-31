using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using System.Data.Entity;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Hei10.Core.Cache;

namespace Hei10.Domain.Repositories
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        public ICacheManager Cache { get; set; }
        public Task<Users> GetByGid(string gid)
        {
            if (string.IsNullOrEmpty(gid))
            {
                return null;
            }
            var arr = gid.Split('-');
            if (arr.Length <2)
            {
                return null;
            }
            var id = StringUtility.ConvertToBigInt(arr[1]);
            if (id == 0)
            {
                return null;
            }
            var obj = GetByIdAsync(id);
            return obj;
        }

        public async Task InitialAdmin()
        {
            if (QueryUnDelete().Any()) return;

            var user = new Users
            {
                CreateUserName = "admin",
                RealName = "管理员",
                HeadImg = "",
                CommonStatus = CommonStatus.Enabled,
                Gender = Gender.Male,
                LoginCount = 0,
                Remark = "系统管理员",
                PasswordSalt = HashUtils.GenerateSalt(),
                LoginName = "admin"
            };
            user.Password = HashUtils.HashPasswordWithSalt("123123", user.PasswordSalt);
            await SaveAsync(user);
        }

        public Task<Users> GetByLoginNameAsync(string loginName)
        {
            var obj = QueryUnDelete()
                .SingleOrDefaultAsync(m => m.LoginName == loginName);
            return obj;
        }

        public async Task<LoginResult> Login(string loginName, string pwd)
        {
            var model = new LoginResult();
            if (string.IsNullOrEmpty(loginName)||string.IsNullOrEmpty(pwd))
            {
                model.Status = SigninStatus.Failed;
                return model;
            }
            var user = await GetByLoginNameAsync(loginName);
            if (user == null)
            {
                model.Status = SigninStatus.UserNotFound;
                return model;
            }
            if (user.CommonStatus == CommonStatus.Disabled)
            {
                model.Status = SigninStatus.Disabled;
                return model;
            }
            if (!user.Password.Equals(HashUtils.HashPasswordWithSalt(pwd,user.PasswordSalt)))
            {
                model.Status = SigninStatus.PasswordIncorrent;
                return model;
            }
            user.LoginCount++;
            //TODO:插入登录日志
            await SaveChangesAsync();

            model.UserInfo = user;
            model.Status = SigninStatus.Succ;
            return model;
        }

        /// <summary>
        /// 返回所有列表，可以查询
        /// </summary>
        /// <param name="keyWord">搜索关键词</param>
        /// <param name="gender">性别</param>
        /// <param name="orderField">排序的字段</param>
        /// <param name="orderDirection">排序的顺序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns></returns>
        public IPagedList<Users> GetPagedList(string keyWord, Gender? gender, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete();
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(m => m.LoginName.Contains(keyWord) ||
                                         m.RealName.Contains(keyWord) || 
                                         m.CreateUserName.Contains(keyWord) || 
                                         m.CreateIP.Contains(keyWord) ||
                                         m.UpdateIP.Contains(keyWord));
            }
            if (gender.HasValue)
            {
                query = query.Where(d => d.Gender == gender.Value);
            }
            
            return ToPageList(query, orderField, orderDirection, m => m.UpdateTime, pageIndex, pageSize);
        }

        

        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        public Task<bool> ExistLoginNameAsync(long id,string loginName)
        {
            return QueryUnDelete().AnyAsync(d => d.Id != id && d.LoginName == loginName);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        public async Task<List<Users>> BatchPasswordResetAsync(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return null;
            }
            var idList = StringUtility.ConvertToBigIntArray(ids, ',');
            if (idList == null || !idList.Any())
            {
                return null;
            }
            var list = QueryUnDelete().Where(d => idList.Any(m => m == d.Id))
                                      .ToList();
            foreach (var item in list)
            {
                item.PasswordSalt = HashUtils.GenerateSalt();
                item.Password = HashUtils.HashPasswordWithSalt("123123", item.PasswordSalt);
                await SaveAsync(item);
            }
            return list; 
        }

        public override void RemoveCache(Users entity)
        {
            base.RemoveCache(entity);
            Cache.Remove(entity.GId);
        }
    }
}
