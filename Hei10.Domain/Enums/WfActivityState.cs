using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum WfActivityState
    {
        [Description("准备")]
        Ready = 0,
        [Description("处理中")]
        Running = 1,
        [Description("已完成")]
        Completed = 2,
        [Description("挂起")]
        Suspended = 3,
        [Description("已撤销")]
        Withdrawed = 4,
        [Description("已退回")]
        Sendbacked = 5
    }
}
