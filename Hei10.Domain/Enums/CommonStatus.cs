using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum CommonStatus
    {
        [Description("删除标记")]
        Deleted = -1,
        [Description("暂存")]
        Disabled = 0,
        [Description("发布")]
        Enabled = 1
    }
}
