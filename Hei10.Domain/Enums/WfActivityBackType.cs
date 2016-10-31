using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum WfActivityBackType
    {
        [Description("退回前一步")]
        PreStep = 1,
        [Description("退回第一步")]
        FirstStep = 2,
        [Description("退回某一步")]
        OtherStep = 3
    }
}
