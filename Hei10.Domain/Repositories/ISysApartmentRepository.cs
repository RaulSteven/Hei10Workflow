using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.Repositories
{
    public interface ISysApartmentRepository:IRepository<SysApartment>
    {
        Task<List<SysApartJsonModel>> GetJsonList();

        Task DeleteAsync(long id);

        /// <summary>
        /// 返回部门所有的记录及部门下所有人员，用于树形结构
        /// </summary>
        /// <param name="userApartmentQuery">部门用户信息</param>
        /// <param name="userQuery">用户信息</param>
        /// <returns></returns>
        Task<List<SysApartZTreeModel>> GetListByZTree(IQueryable<User2Apartment> userApartmentQuery,
            IQueryable<Users> userQuery);
        
        Task<int> GetIndexOfParent(SysApartment apart);
    }
}
