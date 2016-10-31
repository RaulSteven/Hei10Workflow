using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IWfProcessInstanceRepository:IRepository<WfProcessInstance>
    {
        IPagedList<WfProcessInstance> GetApplyPagedList(bool isOwn, string name, string title, DateTime? startTime,
            DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection,
            int pageIndex, int pageSize);

        Task<bool> IsRunAsync(long processId);
    }
}
