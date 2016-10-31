using Hei10.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PagedList;

namespace Hei10.Domain.Services
{
    public class BaseSvc
    {
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

        #region 通用排序

        public IPagedList<TS> ToPageList<TS, TKey>(IQueryable<TS> query,
            string orderField,
            string orderDirection,
            Expression<Func<TS, TKey>> orderFunc,
            int pageIndex,
            int pageSize)
        { 

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
