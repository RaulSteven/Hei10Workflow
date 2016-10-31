using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Extensions;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using Hei10.Domain.Enums;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface ISysOperationLogRepository:IRepository<SysOperationLog>
    {
         Task Insert(TableSource cat, OperationType type, string desc, string srcId);

         Task Insert(TableSource cat, OperationType type, string desc, long srcId);

        IPagedList<SysOperationLog> GetPagedList(int pageCurrent, int pageSize, string orderField, string orderDirection, string source, string sourceId,
            TableSource? cat, OperationType? type, string q, string uname, DateTime? minTime, DateTime? maxTime);
    }
}
