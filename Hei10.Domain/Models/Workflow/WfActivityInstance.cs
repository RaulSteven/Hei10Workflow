using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Hei10.Domain.Models.Workflow
{
    /// <summary>
    /// 流程节点实例
    /// </summary>
    public class WfActivityInstance:AggregateRoot
    {
        public long ProcessInstanceId { get; set; }
        public long ProcessId { get; set; }
        public string ProcessName { get; set; }

        [DisplayName("前节点实例Id")]
        public long PreActInstanceId { get; set; }

        [DisplayName("节点Guid")]
        [Required, MaxLength(60)]
        public string ActivityGuid { get; set; }

        [DisplayName("节点名称")]
        [Required,MaxLength(50)]
        public string ActivityName { get; set; }
        public WfActivityType ActivityType { get; set; }

        public WfActivityState ActivityState { get; set; }

        /// <summary>
        /// 回退的节点Id
        /// </summary>
        public long? BackActivityInstanceId { get; set; }

        /// <summary>
        /// 指派人Id，多个Id以“,”隔开
        /// </summary>
        public string AssignToUserIds { get; set; }
        /// <summary>
        /// 指派人真实姓名，多个姓名以“,”隔开
        /// </summary>
        public string AssignToUserNames { get; set; }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public long DealUserId { get; set; }
        /// <summary>
        /// 处理人真实姓名
        /// </summary>
        public string DealUserName { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? DealTime { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        [DisplayName("意见")]
        [MaxLength(250)]
        public string Comment { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
