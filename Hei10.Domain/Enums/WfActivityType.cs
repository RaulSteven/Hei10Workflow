using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum WfActivityType
    {
        [Description("开始")]
        Start = 1,
        [Description("任务")]
        Task = 2,
        [Description("分支")]
        Switch = 3,
        [Description("结束")]
        End = 4,
    }
}
