using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Hei10.Core.Utilities;
using Hei10.Domain.Enums;

namespace Hei10.Domain.Models.Workflow
{
    /// <summary>
    /// 流程定义类
    /// </summary>
    public partial class WfProcess:AggregateRoot
    {
        [DisplayName("名称")]
        [Required(ErrorMessage =ErrorMsgUtils.Required)]
        [MaxLength(50,ErrorMessage =ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("描述")]
        [MaxLength(250,ErrorMessage =ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 流程内容，Json格式
        /// </summary>
        public string ProcessContent { get; set; }

        /// <summary>
        /// 该流程对应的数据源
        /// </summary>
        public TableSource TableSource { get; set; }

    }
}
