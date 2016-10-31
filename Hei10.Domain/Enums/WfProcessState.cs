using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum WfProcessState
    {
        [Description("未启动")]
        NotStart = 0,
        [Description("准备")]
        Ready = 1,
        [Description("运行中")]
        Running = 2,
        [Description("完成")]
        Completed = 4,
        [Description("挂起")]
        Suspended = 5,
        [Description("取消")]
        Canceled = 6,
        [Description("终止")]
        Discarded = 7
    }
}
