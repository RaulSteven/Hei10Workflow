using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Hei10.Core.Utilities;

namespace Hei10.Domain.Models.Workflow
{
    /// <summary>
    /// 流程实例类
    /// </summary>
    public partial class WfProcessInstance:AggregateRoot
    {
        public long WfProcessId { get; set; }

        [DisplayName("名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string ProcessName { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        public WfProcessState ProcessState { get; set; }


        [Timestamp]
        public Byte[] RowVersion { get; set; }

        /// <summary>
        /// 当前步骤
        /// </summary>
        [DisplayName("当前步骤")]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string CurrActivityName { get; set; }

        /// <summary>
        /// 对应的数据源
        /// </summary>
        public TableSource TableSource { get; set; }
        /// <summary>
        /// 数据源主键值
        /// </summary>
        public long SourceId { get; set; }

        [DisplayName("条件")]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Conditions { get; set; }
    }
}
