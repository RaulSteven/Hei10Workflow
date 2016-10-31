using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum WfActivityDealType
    {
        [Description("发起者")]
        Creater = 1,
        [Description("角色")]
        Role = 2
    }
}
