using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum FilterGroupOp
    {
        [Description("并且")]
        And = 0,
        [Description("或者")]
        Or = 1
    }
}
