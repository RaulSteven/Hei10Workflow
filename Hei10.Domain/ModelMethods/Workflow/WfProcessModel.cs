using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models.Workflow
{
    public class WfProcessModel
    {
        public List<WfActivity> nodeDataArray { get; set; }
        public List<WfTransition> linkDataArray { get; set; }

    }
}
