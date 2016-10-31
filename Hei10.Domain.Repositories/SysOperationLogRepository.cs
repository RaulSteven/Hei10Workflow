using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Core.Extensions;
using Hei10.Core.Utilities;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class SysOperationLogRepository : Repository<SysOperationLog>, ISysOperationLogRepository
    {
        public async Task Insert(TableSource cat, OperationType type, string desc, long srcId)
        {
            await Insert(cat, type, desc, srcId.ToString());
        }

        public async Task Insert(TableSource cat, OperationType type, string desc, string srcId)
        {
            var title = string.Format("{0} {1}", type.GetDescriotion(), cat.GetDescriotion());
            var log = new SysOperationLog
            {
                CommonStatus = CommonStatus.Enabled,
                DataSouceId = srcId,
                DataSource = cat.ToString(),
                LogCat = cat,
                LogDesc = desc,
                LogTitle = title,
                LogType = type
            };
            await SaveAsync(log);
        }

        public IPagedList<SysOperationLog> GetPagedList(int pageCurrent, int pageSize, string orderField, string orderDirection, string source, string sourceId,
            TableSource? cat, OperationType? type, string q, string uname, DateTime? minTime, DateTime? maxTime)
        {
            var query = AdminQueryUnDelete();
            if (!string.IsNullOrEmpty(source))
            {
                query = query.Where(l => l.DataSource.Contains(source));
            }
            if (!string.IsNullOrEmpty(sourceId))
            {
                query = query.Where(l => l.DataSouceId == sourceId);
            }
            if (cat.HasValue)
            {
                query = query.Where(l => l.LogCat == cat.Value);
            }
            if (type.HasValue)
            {
                query = query.Where(l => l.LogType == type);
            }
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(l => l.LogTitle.Contains(q));
            }
            if (!string.IsNullOrEmpty(uname))
            {
                query = query.Where(l => l.CreateUserName == uname);
            }
            if (minTime.HasValue)
            {
                query = query.Where(l => l.CreateTime >= minTime.Value);
            }
            if (maxTime.HasValue)
            {
                query = query.Where(l => l.CreateTime <= maxTime.Value);
            }
            return ToPageList(query, orderField, orderDirection, m => m.CreateTime, pageCurrent, pageSize);
        }
    }
}
