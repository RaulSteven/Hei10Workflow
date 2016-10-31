using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Workflow;

namespace Hei10.Domain.Repositories
{
    public interface IWfActivityInstanceRepository : IRepository<WfActivityInstance>
    {
        Task<WfActivityInstance> GetLatestActivityAsync(long proInsId, string actId);
    }
}
