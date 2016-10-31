using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    [Flags]
    public enum WfActivityButton
    {
        None = 0,
        [Description("回退")]
        FallBack = 1,
        [Description("发送")]
        Send = 2,
        [Description("完成")]
        Complete = 4
    }
}
