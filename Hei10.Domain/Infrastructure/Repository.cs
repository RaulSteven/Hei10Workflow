using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using log4net;
using Hei10.Domain.Entityframework;
using System.Data.Entity;
using System.Web;
using Hei10.Core.Utilities;
using System.Data.Entity.Migrations;
using System.Linq.Dynamic;
using Newtonsoft.Json;
using Hei10.Domain.ViewModels;
using PagedList;
using System.Linq.Expressions;

namespace Hei10.Domain.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        #region fields
        public const string TreePathFormat = "00000";
        public IDbFactory Factory { get; set; }
        public ILog Log
        {
            get;
            private set;
        }

        private IJRUser _User = null;
        public IJRUser User
        {
            get
            {
                if (_User == null)
                {
                    if (HttpContext.Current == null || HttpContext.Current.User == null)
                    {
                        _User = new JRUserEmpty();
                    }
                    else if (HttpContext.Current.User is IJRUser)
                    {
                        _User = (IJRUser)HttpContext.Current.User;
                    }
                    else
                    {
                        _User = new JRUserEmpty();
                    }
                }
                return _User;
            }
        }

        protected DbSet<T> Table
        {
            get
            {
                return Factory.Context.Set<T>();
            }
        }

        private IPUtility _ipUtility = null;

        public string CacheKey = "JR.";
        #endregion

        public Repository()
        {

            #region Log
            Log = LogManager.GetLogger(this.GetType().FullName);
            #endregion
            CacheKey += typeof(T).Name + ".";
        }

        public void Add(T entity)
        {
            if (null == entity) throw new ArgumentNullException("entity");
            this.Table.Add(entity);
        }

        public void Attach(T entity)
        {
            if (null == entity) throw new ArgumentNullException("t");
            Factory.Context.Entry(entity).State = EntityState.Modified;
        }

        public Task<List<T>> BatchDeleteAsync(string ids)
        {
            return BatchStatusAsync(ids, CommonStatus.Deleted);
        }

        public Task<List<T>> BatchStatusAsync(string ids, CommonStatus status, bool isSave = true)
        {
            return BatchStatusAsync(ids, status, null, isSave);
        }

        public async Task<List<T>> BatchStatusAsync(string ids, CommonStatus status, Action<T> batchStatusWork, bool isSave = true)
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
            var list = await QueryUnDelete().Where(d => idList.Contains(d.Id)).ToListAsync();
            if (list == null || !list.Any())
            {
                return null;
            }
            foreach (var item in list)
            {
                RemoveCache(item);
                item.CommonStatus = status;
            }
            if (isSave)
            {
                await SaveChangesAsync();
            }
            return list;
        }

        public virtual async Task Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Table.Remove(entity);

            await SaveChangesAsync();
        }

        public Task<T> GetByIdAsync(long id)
        {
            return QueryAll().SingleOrDefaultAsync(m => m.Id == id);
        }

        public Task<T> GetEnableByIdAsync(long id)
        {
            return QueryEnable().SingleOrDefaultAsync(d => d.Id == id);
        }

        public string GetIP()
        {
            if (_ipUtility == null)
            {
                _ipUtility = new IPUtility();
            }
            return _ipUtility.GetIPAndCity();
        }

        public Task<T> GetUnDeleteByIdAsync(long id)
        {
            return QueryUnDelete().SingleOrDefaultAsync(d => d.Id == id);
        }

        public IQueryable<T> QueryAll()
        {
            return Table.AsQueryable();
        }

        public IQueryable<T> QueryEnable()
        {
            return Table.Where(d => d.CommonStatus == CommonStatus.Enabled);
        }

        public IQueryable<T> QueryUnDelete()
        {
            return Table.Where(d => d.CommonStatus != CommonStatus.Deleted);
        }

        #region 数据权限筛选，专用于后台查询

        private IQueryable<T> AdminQueryFilter(IQueryable<T> query)
        {
            //如无角色数据规则则返回所有
            if (User.UserInfo.UserRoleFilterList == null || !User.UserInfo.UserRoleFilterList.Any())
            {
                return query;
            }
            var entityType = typeof(T);
            var userRoleFilter = User.UserInfo.UserRoleFilterList.FirstOrDefault(m => m.Source == entityType.Name);
            if (userRoleFilter != null && !string.IsNullOrEmpty(userRoleFilter.FilterGroups))//是否有数据规则
            {
                FilterGroup filterGroup = JsonConvert.DeserializeObject<FilterGroup>(userRoleFilter.FilterGroups);
                var filterTranslator = new FilterTranslator(filterGroup, entityType, User);
                filterTranslator.Translate();
                query = query.Where(filterTranslator.CommandText, filterTranslator.ListParams.ToArray());
            }
            return query;
        }
        public IQueryable<T> AdminQueryEnable()
        {
            var query = Table.Where(m => m.CommonStatus == CommonStatus.Enabled);

            return AdminQueryFilter(query);
        }
        public IQueryable<T> AdminQueryAll()
        {
            return AdminQueryFilter(Table.AsQueryable());
        }
        public IQueryable<T> AdminQueryUnDelete()
        {
            var query = Table.Where(m => m.CommonStatus != CommonStatus.Deleted);

            return AdminQueryFilter(query);
        }

        #endregion

        public void Remove(T entity)
        {
            if (null == entity) throw new ArgumentNullException("entity");
            this.Table.Remove(entity);
        }

        public virtual void RemoveCache(T entity)
        {
        }

        public virtual async Task SaveAsync(T entity)
        {
            //Context.Configuration.ValidateOnSaveEnabled = false;
            entity.UpdateTime = DateTime.Now;
            entity.UpdateIP = GetIP();
            entity.UpdateUserId = User.UserInfo.UserId;
            this.Table.AddOrUpdate(entity);
            if (Factory.Context.Entry(entity).State == EntityState.Added)
            {
                //新增
                entity.CreateUserId = User.UserInfo.UserId;
                entity.CreateUserName = User.UserInfo.RealName;
                entity.CreateTime = DateTime.Now;
                entity.CreateIP = GetIP();
            }
            await SaveChangesAsync();

            RemoveCache(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return Factory.SaveChangesAsync();
        }

        #region 通用排序

        public IPagedList<TS> ToPageList<TS, TKey>(IQueryable<TS> query,
            string orderField,
            string orderDirection,
            Expression<Func<TS, TKey>> orderFunc,
            int pageIndex,
            int pageSize)
        {
            //if (string.IsNullOrEmpty(orderField))
            //{
            //    return query.OrderBy(orderFunc).ToPagedList(pageIndex, pageSize);
            //}
            //return query.OrderBy($"{orderField} {orderDirection}")
            //    .ToPagedList(pageIndex, pageSize);


            var queryOrder = string.IsNullOrEmpty(orderField) ? query.OrderBy(orderFunc) : query.OrderBy($"{orderField} {orderDirection}");
            var list = queryOrder.ToPagedList(pageIndex, pageSize);
            if (list.PageCount > 0 && list.PageCount < pageIndex)
            {
                list = queryOrder.ToPagedList(list.PageCount, pageSize);
            }
            return list;
            ;
        }
        #endregion
    }
}
