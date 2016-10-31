using Hei10.Domain.Entityframework;
using Hei10.Domain.Enums;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Infrastructure
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IDbFactory Factory { get; set; }
        Task<T> GetByIdAsync(long id);

        IJRUser User { get; }

        Task SaveAsync(T entity);

        Task<int> SaveChangesAsync();

        /// <summary>
        /// 带SaveChanges的删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Delete(T entity);

        /// <summary>
        /// 不带SaveChanges的删除
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);
        /// <summary>
        /// 不带SaveChanges，需要手工保存
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        void RemoveCache(T entity);

        Task<List<T>> BatchDeleteAsync(string ids);

        Task<List<T>> BatchStatusAsync(string ids, CommonStatus status, bool isSave = true);
        Task<List<T>> BatchStatusAsync(string ids, CommonStatus status, Action<T> batchStatusWork, bool isSave = true);

        ILog Log { get; }

        string GetIP();

        /// <summary>
        /// 通过Id返回一条已发布的记录
        /// </summary>
        /// <param name="id">当前记录的Id</param>
        /// <returns></returns>
        Task<T> GetEnableByIdAsync(long id);
        /// <summary>
        /// 通过Id返回一条未删除的记录
        /// </summary>
        /// <param name="id">当前记录的Id</param>
        /// <returns></returns>
        Task<T> GetUnDeleteByIdAsync(long id);

        /// <summary>
        /// 返回所有的记录
        /// </summary>
        /// <returns></returns>
        IQueryable<T> QueryAll();

        /// <summary>
        /// 返回所有已发布的记录
        /// </summary>
        /// <returns></returns>
        IQueryable<T> QueryEnable();

        /// <summary>
        /// 返回所有未删除的记录，包含已发布的和暂存的
        /// </summary>
        /// <returns></returns>
        IQueryable<T> QueryUnDelete();

        #region 后台查询，数据权限筛选

        /// <summary>
        /// 返回所有的记录
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AdminQueryAll();

        /// <summary>
        /// 返回所有已发布的记录
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AdminQueryEnable();

        /// <summary>
        /// 返回所有未删除的记录，包含已发布的和暂存的
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AdminQueryUnDelete();

        #endregion

        void Attach(T t);

    }
}
