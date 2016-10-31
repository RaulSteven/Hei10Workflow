using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class WfActivityInstanceRepository : Repository<WfActivityInstance>, IWfActivityInstanceRepository
    {
        public Task<WfActivityInstance> GetLatestActivityAsync(long proInsId, string actId)
        {
            var act = AdminQueryEnable()
                .Where(m => m.ProcessInstanceId == proInsId
                    && m.ActivityGuid == actId)
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();
            return act;
        }
    }
}
