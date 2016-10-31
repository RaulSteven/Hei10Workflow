using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum ConsultType
    {
        [Description("工程咨询")]
        Project,
        [Description("造价咨询")]
        Cost
    }
}
