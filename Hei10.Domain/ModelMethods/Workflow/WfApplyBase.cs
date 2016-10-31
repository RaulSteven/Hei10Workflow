using Hei10.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models.Workflow
{
    public class WfApplyBase
    {
        public long Id { get; set; }

        /// <summary>
        /// 流程Id
        /// </summary>
        public long ProcessId { get; set; }

        /// <summary>
        /// 下一步节点Id
        /// </summary>
        public string NextStepId { get; set; }

        /// <summary>
        /// 下一步节点审核人Id
        /// </summary>
        public string AssignToUserIds { get; set; }

        /// <summary>
        /// 下一步节点审核人真实姓名
        /// </summary>
        public string AssignToUserNames { get; set; }

        #region 回退后重新申请信息
        /// <summary>
        /// 重新申请时流程实例Id
        /// </summary>
        public long ProcessInstanceId { get; set; }
        /// <summary>
        /// 重新申请时，当前节点实例Id
        /// </summary>
        public long CurrActInsId { get; set; }
        /// <summary>
        /// 审核历史记录
        /// </summary>
        public List<WfActivityInstance> DealActivityList { get; set; }
        #endregion

        public TableSource TableSource { get; set; }
    }
}
