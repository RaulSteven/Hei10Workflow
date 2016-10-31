using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
namespace Hei10.Domain.Models.Workflow
{
    /// <summary>
    /// 流程节点
    /// </summary>
    public class WfActivity
    {
        public string key { get; set; }
        public string text { get; set; }

        public WfActivityType activityType { get; set; }

        /// <summary>
        /// 处理类型
        /// </summary>
        public WfActivityDealType dealType { get; set; }

        /// <summary>
        /// 角色id列表，英文“,”隔开
        /// </summary>
        public string roleIds { get; set; }

        /// <summary>
        /// 回退类型
        /// </summary>
        public WfActivityBackType backType { get; set; }

        /// <summary>
        /// 回退步骤的key
        /// </summary>
        public string backStep { get; set; }
    }
}
