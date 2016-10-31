using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum OperationType
    {
        [Description("默认")]
        Default = 0,
        [Description("用户登录")]
        UserLogin = 1,
        [Description("插入")]
        Insert = 2,
        [Description("更新")]
        Update = 3,
        [Description("删除")]
        Delete = 4
    }
}
