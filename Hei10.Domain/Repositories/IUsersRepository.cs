using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Enums;
using Hei10.Domain.ViewModels;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IUsersRepository:IRepository<Users>
    {
        Task<Users> GetByGid(string gid);

        Task InitialAdmin();

        Task<Users> GetByLoginNameAsync(string loginName);

        Task<LoginResult> Login(string loginName, string pwd);

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
        IPagedList<Users> GetPagedList(string keyWord, Gender? gender, string orderField, string orderDirection, int pageIndex, int pageSize);

        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        Task<bool> ExistLoginNameAsync(long id, string loginName);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        Task<List<Users>> BatchPasswordResetAsync(string ids);
    }
}
