using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;
using System.Data.Entity;
using Hei10.Domain.Enums;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class WfProcessInstanceRepository : Repository<WfProcessInstance>, IWfProcessInstanceRepository
    {
        public IPagedList<WfProcessInstance> GetApplyPagedList(bool isOwn,string name, string title, DateTime? startTime, DateTime? endTime, WfProcessState? state, string activityName, string orderField, string orderDirection, int pageIndex, int pageSize)
        {
            var query = AdminQueryUnDelete();
            if (isOwn)
            {
                query = query
                    .Where(m => m.CreateUserId == User.UserInfo.UserId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.CreateUserName.Contains(name));
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(m => m.ProcessName.Contains(title));
            }
            if (startTime.HasValue)
            {
                query = query.Where(l => l.CreateTime >= startTime.Value);
            }
            if (endTime.HasValue)
            {
                query = query.Where(l => l.CreateTime <= endTime.Value);
            }
            if (state.HasValue)
            {
                query = query.Where(m => m.ProcessState == state.Value);
            }
            if (!string.IsNullOrEmpty(activityName))
            {
                query = query.Where(m => m.CurrActivityName.Contains(activityName));
            }
            query = query.OrderByDescending(m => m.Id);
            return ToPageList(query, orderField, orderDirection, m => m.UpdateTime, pageIndex, pageSize);
        }

        public Task<bool> IsRunAsync(long processId)
        {
            return
                AdminQueryUnDelete()
                    .AnyAsync(
                        m =>
                            m.WfProcessId == processId &&
                            (m.ProcessState == WfProcessState.Ready || m.ProcessState == WfProcessState.Running));
        }
    }
}
